using System;
using System.Diagnostics;
using Godot;
namespace JigglePhysics; 

// Uses Verlet to resolve constraints easily 
public class JiggleBone {
	private PositionSignal _targetAnimatedBoneSignal;
	private Vector3 _currentFixedAnimatedBonePosition;

	public readonly JiggleBone Parent;
	private JiggleBone _child;

	private readonly Skeleton3D _targetSkeleton;
	private int? _boneId;

	private Transform3D Transform {
		get {
			Debug.Assert(_boneId != null, nameof(_boneId) + " != null");
			return _targetSkeleton.GetBoneGlobalPose(_boneId.Value);
		}
	}

	public Transform3D GlobalTransform => _targetSkeleton.GlobalTransform * Transform;

	public Transform3D ParentGlobalTransform {
		get {
#if DEBUG
			if (Parent != null) {
				return Parent.GlobalTransform;
			}

			if (!_boneId.HasValue) {
				throw new Exception(
					"This should never happen, no parent but also no bone??? we're just a single floating virtual node with no associations??");
			}
#endif
			var parentBoneId = _targetSkeleton.GetBoneParent(_boneId.Value);
			if (parentBoneId == -1) {
				return _targetSkeleton.GlobalTransform;
			}
			return _targetSkeleton.GlobalTransform * _targetSkeleton.GetBoneGlobalPose(parentBoneId);
		}
	}

	private Vector3 _workingPosition;
	private PositionSignal _particleSignal;

	private Vector3 _preTeleportPosition;

	private Vector3 _extrapolatedPosition;

	private float GetLengthToParent() {
		if (Parent == null) {
			return 0.1f;
		}
		return _currentFixedAnimatedBonePosition.DistanceTo(Parent._currentFixedAnimatedBonePosition);
	}
	
	public JiggleBone(Skeleton3D skeleton, int? boneId, JiggleBone parent, Vector3 position) {
		_targetSkeleton = skeleton;
		_boneId = boneId;
		Parent = parent;
		var updateTime = Time.GetTicksUsec();
		_targetAnimatedBoneSignal = new PositionSignal(position, updateTime);
		_particleSignal = new PositionSignal(position, updateTime);
		
		if (parent == null) {
			return;
		}

		Parent._child = this;
	}

	private float CalculateNormalizedIndex() {
		int distanceToRoot = 0;
		JiggleBone test = this;
		while (test.Parent != null) {
			test = test.Parent;
			distanceToRoot++;
		}

		int distanceToChild = 0;
		test = this;
		while (test._child != null) {
			test = test._child;
			distanceToChild++;
		}

		int max = distanceToRoot + distanceToChild;
		float frac = (float)distanceToRoot / max;
		return frac;
	}

	public void FirstPass(JiggleData jiggleData, Vector3 wind, ulong time, ulong delta) {
		const ulong secondsToMicroseconds = 1000000;
		float fdelta = (float)delta / (float)secondsToMicroseconds;
		_currentFixedAnimatedBonePosition = _targetAnimatedBoneSignal.SamplePosition(time);
		if (Parent == null) {
			_workingPosition = _currentFixedAnimatedBonePosition;
			_particleSignal.SetPosition(_currentFixedAnimatedBonePosition, time);
			return;
		}
		Vector3 localSpaceVelocity = (_particleSignal.GetCurrent()-_particleSignal.GetPrevious()) - (Parent._particleSignal.GetCurrent()-Parent._particleSignal.GetPrevious());
		_workingPosition = NextPhysicsPosition(
			_particleSignal.GetCurrent(), _particleSignal.GetPrevious(), localSpaceVelocity, fdelta,
			jiggleData.GravityMultiplier,
			jiggleData.Friction,
			jiggleData.AirFriction
		);
		_workingPosition += wind * (fdelta * jiggleData.AirFriction);
	}

	public void SecondPass(JiggleData jiggleData, ulong time) {
		_workingPosition = ConstrainLengthBackwards(_workingPosition, jiggleData.LengthElasticity*jiggleData.LengthElasticity*0.5f);
	}

	public void ThirdPass(JiggleData jiggleData, ulong time) {
		if (Parent == null) {
			return;
		}
		_workingPosition = ConstrainAngle(_workingPosition, jiggleData.AngleElasticity*jiggleData.AngleElasticity, jiggleData.ElasticitySoften); 
		_workingPosition = ConstrainLength(_workingPosition, jiggleData.LengthElasticity*jiggleData.LengthElasticity);
		_particleSignal.SetPosition(_workingPosition, time);
	}
	private Vector3 GetProjectedPosition() {
		if (_boneId.HasValue) {
			throw new Exception("Tried to get a projected position of a JiggleBone that doesn't need to project!");
		}
		// TODO: This can work entirely in skeleton space, before transforming it to a global space.
		return ParentGlobalTransform * (Parent.ParentGlobalTransform.Inverse() * ParentGlobalTransform.Origin);
	}

	private void CacheAnimationPosition(ulong time) {
		if (!_boneId.HasValue) {
			_targetAnimatedBoneSignal.SetPosition(GetProjectedPosition(), time);
			return;
		}
		_targetAnimatedBoneSignal.SetPosition(GlobalTransform.Origin, time);
	}

	private Vector3 ConstrainLengthBackwards(Vector3 newPosition, float elasticity) {
		if (_child == null) {
			return newPosition;
		}
		Vector3 diff = newPosition - _child._workingPosition;
		Vector3 dir = diff.Normalized();
		return newPosition.Lerp(_child._workingPosition + dir * _child.GetLengthToParent(), elasticity);
	}

	private Vector3 ConstrainLength(Vector3 newPosition, float elasticity) {
		Vector3 diff = newPosition - Parent._workingPosition;
		Vector3 dir = diff.Normalized();
		return newPosition.Lerp(Parent._workingPosition + dir * GetLengthToParent(), elasticity);
	}

	private Vector3 ConstrainAngle(Vector3 newPosition, float elasticity, float elasticitySoften) {
		Vector3 poseParentParent;
		Vector3 parentParentPosition;
		if (Parent.Parent == null) {
			poseParentParent = Parent._currentFixedAnimatedBonePosition + (Parent._currentFixedAnimatedBonePosition - _currentFixedAnimatedBonePosition);
			parentParentPosition = poseParentParent;
		} else {
			parentParentPosition = Parent.Parent._workingPosition;
			poseParentParent = Parent.Parent._currentFixedAnimatedBonePosition;
		}
		Vector3 parentAimTargetPose = Parent._currentFixedAnimatedBonePosition - poseParentParent;
		Vector3 parentAim = Parent._workingPosition - parentParentPosition;
		Quaternion targetPoseToPose = new Quaternion(parentAimTargetPose.Normalized(),parentAim.Normalized()).Normalized();
		Vector3 currentPose = _currentFixedAnimatedBonePosition - poseParentParent;
		Vector3 constraintTarget = targetPoseToPose * currentPose;
		float error = newPosition.DistanceTo(parentParentPosition + constraintTarget);
		error /= GetLengthToParent();
		error = Mathf.Clamp(error, 0f, 1f);
		error = Mathf.Pow(error, elasticitySoften * 2f);
		return newPosition.Lerp(parentParentPosition + constraintTarget, elasticity * error);
	}
	
	public static Vector3 NextPhysicsPosition(Vector3 newPosition, Vector3 previousPosition, Vector3 localSpaceVelocity, float deltaTime, float gravityMultiplier, float friction, float airFriction) {
		float squaredDeltaTime = deltaTime * deltaTime;
		Vector3 vel = newPosition - previousPosition - localSpaceVelocity;
		// TODO: Godot has area-specific gravity, need to integrate with that.
		Vector3 gravity = Vector3.Down * 9.81f;
		return newPosition + vel * (1f - airFriction) + localSpaceVelocity * (1f - friction) + gravity * (gravityMultiplier * squaredDeltaTime);
	}

	public Vector3 DeriveFinalSolvePosition(ulong time) {
		_extrapolatedPosition = _particleSignal.SamplePosition(time);
		return _extrapolatedPosition;
	}

	public void SampleBone(ulong time) {
		CacheAnimationPosition(time);
	}

	/*public void DrawDebug(ulong time) {
		Vector3 positionBlend = _extrapolatedPosition;
		if (_child == null) return;
		Vector3 childPositionBlend = _child._extrapolatedPosition;
		Vector3 dir = (childPositionBlend - positionBlend).Normalized();
		DebugDraw3D.DrawLine(positionBlend, childPositionBlend+dir, Colors.Yellow);
		Vector3 dir2 = (_child._targetAnimatedBoneSignal.SamplePosition(time) - _targetAnimatedBoneSignal.SamplePosition(time)).Normalized();
		DebugDraw3D.DrawLine(_targetAnimatedBoneSignal.SamplePosition(time), _child._targetAnimatedBoneSignal.SamplePosition(time)+dir2, Colors.Green);
	}*/

	public void ResetBone() {
		if (_boneId.HasValue) {
			_targetSkeleton.ResetBonePose(_boneId.Value);
		}
	}

	public void PoseBone(float blend, ulong time) {
		if (_child != null) {
			Vector3 positionBlend = _targetAnimatedBoneSignal.SamplePosition(time).Lerp(_extrapolatedPosition, blend);
			Vector3 childPositionBlend = _child._targetAnimatedBoneSignal.SamplePosition(time).Lerp(_child._extrapolatedPosition, blend);

			var inverseParent = (ParentGlobalTransform).Inverse();

			if (Parent != null) {
				_targetSkeleton.SetBonePosePosition(_boneId.Value, inverseParent * positionBlend);
			}

			Vector3 childPosition;
			if (!_child._boneId.HasValue) {
				childPosition = GlobalTransform * (ParentGlobalTransform.Inverse() * GlobalTransform.Origin);
			} else {
				childPosition = _child.GlobalTransform.Origin;
			}

			Vector3 cachedAnimatedVector = childPosition - GlobalTransform.Origin;
			Vector3 simulatedVector = childPositionBlend - positionBlend;
			var localAnimatedVector = inverseParent.Basis * cachedAnimatedVector;
			var localSimulatedVector = inverseParent.Basis * simulatedVector;
			Quaternion animPoseToPhysicsPose = new Quaternion(localAnimatedVector.Normalized(), localSimulatedVector.Normalized());
			Debug.Assert(_boneId != null, nameof(_boneId) + " != null");
			_targetSkeleton.SetBonePoseRotation(_boneId.Value,animPoseToPhysicsPose*_targetSkeleton.GetBonePoseRotation(_boneId.Value));
			// FIXME: The last bone set sometimes doesn't trigger an update. Godot bug?? This forces it to update.
			if (_child._child == null) {
				_targetSkeleton.GetBoneGlobalPose(_boneId.Value);
			}
		}
		//if (_boneId.HasValue) {
			//_targetSkeleton.GetBoneGlobalPose(_boneId.Value);
		//}
	}
}