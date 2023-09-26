using System;
using System.Diagnostics;
using Godot;
namespace JigglePhysics; 

// Uses Verlet to resolve constraints easily 
public class JiggleBone {
	private struct PositionFrame {
		public readonly Vector3 Position;
		public readonly ulong Time;
		public PositionFrame(Vector3 position, ulong time) {
			this.Position = position;
			this.Time = time;
		}
	}
	
	private PositionFrame _currentTargetAnimatedBoneFrame;
	private PositionFrame _lastTargetAnimatedBoneFrame;
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

	private ulong _updateTime;
	private ulong _previousUpdateTime;
	
	public Vector3 Position;
	private Vector3 _previousPosition;

	private Vector3 _preTeleportPosition;

	private Vector3 _extrapolatedPosition;

	private float GetLengthToParent() {
		if (Parent == null) {
			return 0.1f;
		}
		return _currentFixedAnimatedBonePosition.DistanceTo(Parent._currentFixedAnimatedBonePosition);
	}
	
	private static Vector3 GetTargetBonePosition(PositionFrame prev, PositionFrame next, ulong time) {
		var diff = next.Time - prev.Time;
		if (diff == 0) {
			return prev.Position;
		}
		double t = ((double)time - (double)prev.Time) / (double)diff;
		return prev.Position.Lerp(next.Position, (float)t);
	}
	
	public JiggleBone(Skeleton3D skeleton, int? boneId, JiggleBone parent, Vector3 position) {
		_targetSkeleton = skeleton;
		_boneId = boneId;
		Parent = parent;
		Position = position;
		_previousPosition = position;
		
		if (boneId.HasValue) {
		}

		_updateTime = Time.GetTicksUsec();
		_previousUpdateTime = _updateTime;
		_lastTargetAnimatedBoneFrame = new PositionFrame(position, _updateTime);
		_currentTargetAnimatedBoneFrame = _lastTargetAnimatedBoneFrame;

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

	public void FirstPass(JiggleData jiggleData, Vector3 wind, ulong time, float deltaTime) {
		_currentFixedAnimatedBonePosition = GetTargetBonePosition(_lastTargetAnimatedBoneFrame, _currentTargetAnimatedBoneFrame, time);
		if (Parent == null) {
			RecordPosition(time);
			Position = _currentFixedAnimatedBonePosition;
			return;
		}
		Vector3 localSpaceVelocity = (Position-_previousPosition) - (Parent.Position-Parent._previousPosition);
		Vector3 newPosition = NextPhysicsPosition(
			Position, _previousPosition, localSpaceVelocity, deltaTime,
			jiggleData.GravityMultiplier,
			jiggleData.Friction,
			jiggleData.AirFriction
		);
		newPosition += wind * (deltaTime * jiggleData.AirFriction);
		RecordPosition(time);
		Position = newPosition;
	}

	public void SecondPass(JiggleData jiggleData) {
		Position = ConstrainLengthBackwards(Position, jiggleData.LengthElasticity*jiggleData.LengthElasticity*0.5f);
	}

	public void ThirdPass(JiggleData jiggleData) {
		if (Parent == null) {
			return;
		}
		Position = ConstrainAngle(Position, jiggleData.AngleElasticity*jiggleData.AngleElasticity, jiggleData.ElasticitySoften); 
		Position = ConstrainLength(Position, jiggleData.LengthElasticity*jiggleData.LengthElasticity);
	}
	private Vector3 GetProjectedPosition() {
		if (_boneId.HasValue) {
			throw new Exception("Tried to get a projected position of a JiggleBone that doesn't need to project!");
		}
		// TODO: This can work entirely in skeleton space, before transforming it to a global space.
		return ParentGlobalTransform * (Parent.ParentGlobalTransform.Inverse() * ParentGlobalTransform.Origin);
	}

	private void CacheAnimationPosition() {
		// Purely virtual particles need to reconstruct their desired position.
		_lastTargetAnimatedBoneFrame = _currentTargetAnimatedBoneFrame;
		if (!_boneId.HasValue) {
			_currentTargetAnimatedBoneFrame = new PositionFrame(GetProjectedPosition(), Time.GetTicksUsec());
			return;
		}
		_currentTargetAnimatedBoneFrame = new PositionFrame(GlobalTransform.Origin, Time.GetTicksUsec());
	}

	private Vector3 ConstrainLengthBackwards(Vector3 newPosition, float elasticity) {
		if (_child == null) {
			return newPosition;
		}
		Vector3 diff = newPosition - _child.Position;
		Vector3 dir = diff.Normalized();
		return newPosition.Lerp(_child.Position + dir * _child.GetLengthToParent(), elasticity);
	}

	private Vector3 ConstrainLength(Vector3 newPosition, float elasticity) {
		Vector3 diff = newPosition - Parent.Position;
		Vector3 dir = diff.Normalized();
		return newPosition.Lerp(Parent.Position + dir * GetLengthToParent(), elasticity);
	}

	public void PrepareTeleport() {
		if (!_boneId.HasValue) {
			Vector3 parentTransformPosition = Parent.GlobalTransform.Origin;
			_preTeleportPosition = Parent.GlobalTransform * (Parent.ParentGlobalTransform.Inverse() * parentTransformPosition);
			return;
		}
		_preTeleportPosition = GlobalTransform.Origin;
	}
	
	public void FinishTeleport() {
		Vector3 teleportedPosition;
		if (!_boneId.HasValue) {
			Vector3 parentTransformPosition = Parent.GlobalTransform.Origin;
			teleportedPosition = Parent.GlobalTransform * Parent.ParentGlobalTransform.Inverse() * parentTransformPosition;
		} else {
			teleportedPosition = GlobalTransform.Origin;
		}
		Vector3 diff = teleportedPosition - _preTeleportPosition;
		_lastTargetAnimatedBoneFrame = new PositionFrame(_lastTargetAnimatedBoneFrame.Position + diff, _lastTargetAnimatedBoneFrame.Time);
		_currentTargetAnimatedBoneFrame = new PositionFrame(_currentTargetAnimatedBoneFrame.Position + diff, _currentTargetAnimatedBoneFrame.Time);
		Position += diff;
		_previousPosition += diff;
	}

	private Vector3 ConstrainAngle(Vector3 newPosition, float elasticity, float elasticitySoften) {
		Vector3 poseParentParent;
		Vector3 parentParentPosition;
		if (Parent.Parent == null) {
			poseParentParent = Parent._currentFixedAnimatedBonePosition + (Parent._currentFixedAnimatedBonePosition - _currentFixedAnimatedBonePosition);
			parentParentPosition = poseParentParent;
		} else {
			parentParentPosition = Parent.Parent.Position;
			poseParentParent = Parent.Parent._currentFixedAnimatedBonePosition;
		}
		Vector3 parentAimTargetPose = Parent._currentFixedAnimatedBonePosition - poseParentParent;
		Vector3 parentAim = Parent.Position - parentParentPosition;
		Quaternion targetPoseToPose = new Quaternion(parentAimTargetPose.Normalized(),parentAim.Normalized()).Normalized();
		Vector3 currentPose = _currentFixedAnimatedBonePosition - poseParentParent;
		Vector3 constraintTarget = targetPoseToPose * currentPose;
		float error = newPosition.DistanceTo(parentParentPosition + constraintTarget);
		error /= GetLengthToParent();
		error = Mathf.Clamp(error, 0f, 1f);
		error = Mathf.Pow(error, elasticitySoften * 2f);
		return newPosition.Lerp(parentParentPosition + constraintTarget, elasticity * error);
	}

	private void RecordPosition(ulong time) {
		_previousUpdateTime = _updateTime;
		_previousPosition = Position;
		_updateTime = time;
	}

	public static Vector3 NextPhysicsPosition(Vector3 newPosition, Vector3 previousPosition, Vector3 localSpaceVelocity, float deltaTime, float gravityMultiplier, float friction, float airFriction) {
		float squaredDeltaTime = deltaTime * deltaTime;
		Vector3 vel = newPosition - previousPosition - localSpaceVelocity;
		// TODO: Godot has area-specific gravity, need to integrate with that.
		Vector3 gravity = Vector3.Down * 9.81f;
		return newPosition + vel * (1f - airFriction) + localSpaceVelocity * (1f - friction) + gravity * (gravityMultiplier * squaredDeltaTime);
	}

	public Vector3 DeriveFinalSolvePosition(Vector3 offset, float smoothing, ulong deltaTime) {
		float smoothAmount = (smoothing * deltaTime);
		double t = ((Time.GetTicksUsec() - smoothAmount) - _previousUpdateTime) / (double)deltaTime;
		_extrapolatedPosition = offset+_previousPosition.Lerp(Position, (float)t);
		return _extrapolatedPosition;
	}

	public void PrepareBone() {
		// If bone is not animated, return to last unadulterated pose
		if (_boneId.HasValue) {
		}
		CacheAnimationPosition();
	}

	public void DrawDebug(float simulated) {
		Vector3 positionBlend = _currentTargetAnimatedBoneFrame.Position.Lerp(_extrapolatedPosition, simulated);
		if (_child != null) {
			Vector3 childPositionBlend = _child._currentTargetAnimatedBoneFrame.Position.Lerp(_child._extrapolatedPosition, simulated);
			DebugDraw3D.DrawLine(positionBlend, childPositionBlend, Colors.Yellow);
		}
		if (_boneId.HasValue) {
			DebugDraw3D.DrawLine(GlobalTransform.Origin, ParentGlobalTransform.Origin, Colors.Green);
		}
	}

	public void ResetBone() {
		if (_boneId.HasValue) {
			_targetSkeleton.ResetBonePose((_boneId.Value));
		}
	}

	public void PoseBone(float blend) {
		if (_child != null) {
			Vector3 positionBlend = _currentTargetAnimatedBoneFrame.Position.Lerp(_extrapolatedPosition, blend);
			Vector3 childPositionBlend = _child._currentTargetAnimatedBoneFrame.Position.Lerp(_child._extrapolatedPosition, blend);

			var inverseParent = (ParentGlobalTransform).Inverse();

			if (Parent != null) {
				_targetSkeleton.SetBonePosePosition(_boneId??-1, inverseParent * positionBlend);
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
		}
		if (_boneId.HasValue) {
			_targetSkeleton.GetBoneGlobalPose(_boneId.Value);
		}
	}
}