using System;
using System.Collections;
using System.Collections.Generic;
using Godot;
namespace JigglePhysics; 

// Uses Verlet to resolve constraints easily 
public class JiggleBone {
	private struct PositionFrame {
		public Vector3 position;
		public ulong time;
		public PositionFrame(Vector3 position, ulong time) {
			this.position = position;
			this.time = time;
		}
	}
	
	private PositionFrame currentTargetAnimatedBoneFrame;
	private PositionFrame lastTargetAnimatedBoneFrame;
	private Vector3 currentFixedAnimatedBonePosition;

	public JiggleBone parent;
	public JiggleBone child;
	private Transform3D boneTransformChangeCheck;
	public Transform3D lastValidLocalPose;
	private float normalizedIndex;
	//public Vector3 targetAnimatedBonePosition;

	private Skeleton3D targetSkeleton;
	private int? boneID;
	public Transform3D Transform => targetSkeleton.GetBoneGlobalPose(boneID.Value);
	public Transform3D GlobalTransform => targetSkeleton.GlobalTransform * Transform;

	public Transform3D ParentGlobalTransform {
		get {
#if DEBUG
			if (parent != null) {
				return parent.GlobalTransform;
			}

			if (!boneID.HasValue) {
				throw new Exception(
					"This should never happen, no parent but also no bone??? we're just a single floating virtual node with no associations??");
			}
#endif
			int parentBoneId = targetSkeleton.GetBoneParent(boneID.Value);
			if (parentBoneId == -1) {
				return targetSkeleton.GlobalTransform;
			}
			return targetSkeleton.GlobalTransform * targetSkeleton.GetBoneGlobalPose(parentBoneId);
		}
	}

	private ulong updateTime;
	private ulong previousUpdateTime;
	
	public Vector3 position;
	public Vector3 previousPosition;
	
	public Vector3 preTeleportPosition;

	private Vector3 extrapolatedPosition;

	// Optimized out, faster to cache once during PrepareBone and reuse.
	/*public Vector3 interpolatedPosition {
		get {
			// extrapolation, because interpolation is delayed by fixedDeltaTime
			float timeSinceLastUpdate = Time.time-Time.fixedTime;
			return Vector3.Lerp(position, position+(position-previousPosition), timeSinceLastUpdate/Time.fixedDeltaTime);

			// Interpolation
			//return Vector3.Lerp(previousPosition, position, timeSinceLastUpdate/Time.fixedDeltaTime);
		}
	}*/
	
	private float GetLengthToParent() {
		if (parent == null) {
			return 0.1f;
		}
		return currentFixedAnimatedBonePosition.DistanceTo(parent.currentFixedAnimatedBonePosition);
	}
	
	private Vector3 GetTargetBonePosition(PositionFrame prev, PositionFrame next, ulong time) {
		ulong diff = next.time - prev.time;
		if (diff == 0) {
			return prev.position;
		}
		double t = ((double)time - (double)prev.time) / (double)diff;
		return prev.position.Lerp(next.position, (float)t);
	}
	
	public JiggleBone(Skeleton3D skeleton, int? boneID, JiggleBone parent, Vector3 position) {
		targetSkeleton = skeleton;
		this.boneID = boneID;
		this.parent = parent;
		this.position = position;
		previousPosition = position;
		
		if (boneID.HasValue) {
			lastValidLocalPose = Transform;
		}

		updateTime = Time.GetTicksUsec();
		previousUpdateTime = updateTime;
		lastTargetAnimatedBoneFrame = new PositionFrame(position, updateTime);
		currentTargetAnimatedBoneFrame = lastTargetAnimatedBoneFrame;

		if (parent == null) {
			return;
		}

		//previousLocalPosition = parent.transform.InverseTransformPoint(previousPosition);
		this.parent.child = this;
	}

	public void CalculateNormalizedIndex() {
		int distanceToRoot = 0;
		JiggleBone test = this;
		while (test.parent != null) {
			test = test.parent;
			distanceToRoot++;
		}

		int distanceToChild = 0;
		test = this;
		while (test.child != null) {
			test = test.child;
			distanceToChild++;
		}

		int max = distanceToRoot + distanceToChild;
		float frac = (float)distanceToRoot / max;
		normalizedIndex = frac;
	}

	public void FirstPass(JiggleSettingsBase jiggleSettings, Vector3 wind, ulong time, float deltaTime) {
		currentFixedAnimatedBonePosition = GetTargetBonePosition(lastTargetAnimatedBoneFrame, currentTargetAnimatedBoneFrame, time);
		if (parent == null) {
			RecordPosition(time);
			position = currentFixedAnimatedBonePosition;
			return;
		}
		Vector3 localSpaceVelocity = (position-previousPosition) - (parent.position-parent.previousPosition);
		Vector3 newPosition = NextPhysicsPosition(
			position, previousPosition, localSpaceVelocity, deltaTime,
			jiggleSettings.GetParameter(JiggleSettingsBase.JiggleSettingParameter.Gravity),
			jiggleSettings.GetParameter(JiggleSettingsBase.JiggleSettingParameter.Friction),
			jiggleSettings.GetParameter(JiggleSettingsBase.JiggleSettingParameter.AirFriction)
		);
		newPosition += wind * (deltaTime * jiggleSettings.GetParameter(JiggleSettingsBase.JiggleSettingParameter.AirFriction));
		RecordPosition(time);
		position = newPosition;
	}

	public void SecondPass(JiggleSettingsBase jiggleSettings) {
		//position = ConstrainLengthBackwards(position, jiggleSettings.GetParameter(JiggleSettingsBase.JiggleSettingParameter.LengthElasticity)*jiggleSettings.GetParameter(JiggleSettingsBase.JiggleSettingParameter.LengthElasticity)*0.5f);
	}

	public void ThirdPass(JiggleSettingsBase jiggleSettings) {
		if (parent == null) {
			return;
		}
		position = ConstrainAngle(position, jiggleSettings.GetParameter(JiggleSettingsBase.JiggleSettingParameter.AngleElasticity)*jiggleSettings.GetParameter(JiggleSettingsBase.JiggleSettingParameter.AngleElasticity), jiggleSettings.GetParameter(JiggleSettingsBase.JiggleSettingParameter.ElasticitySoften)); 
		position = ConstrainLength(position, jiggleSettings.GetParameter(JiggleSettingsBase.JiggleSettingParameter.LengthElasticity)*jiggleSettings.GetParameter(JiggleSettingsBase.JiggleSettingParameter.LengthElasticity));
	}

	/*public void FinalPass(JiggleSettingsBase jiggleSettings, ulong time, ICollection<Collider> colliders) {
		if (!CachedSphereCollider.TryGet(out SphereCollider sphereCollider)) return;
		sphereCollider.enabled = true;
		foreach (var collider in colliders) {
			sphereCollider.radius = jiggleSettings.GetRadius(normalizedIndex);
			if (sphereCollider.radius <= 0) {
				continue;
			}

			if (Physics.ComputePenetration(sphereCollider, position, Quaternion.identity,
					collider, collider.transform.position, collider.transform.rotation,
					out Vector3 dir, out float dist)) {
				position += dir * dist;
			}
		}
		sphereCollider.enabled = false;
	}*/


	private Vector3 GetProjectedPosition() {
		if (boneID.HasValue) {
			throw new Exception("Tried to get a projected position of a JiggleBone that doesn't need to project!");
		}
		// TODO: This can work entirely in skeleton space, before transforming it to a global space.
		return ParentGlobalTransform * (parent.ParentGlobalTransform.Inverse() * ParentGlobalTransform.Origin);
	}

	public void CacheAnimationPosition() {
		// Purely virtual particles need to reconstruct their desired position.
		lastTargetAnimatedBoneFrame = currentTargetAnimatedBoneFrame;
		if (!boneID.HasValue) {
			currentTargetAnimatedBoneFrame = new PositionFrame(GetProjectedPosition(), Time.GetTicksUsec());
			return;
		}
		currentTargetAnimatedBoneFrame = new PositionFrame(GlobalTransform.Origin, Time.GetTicksUsec());
		lastValidLocalPose = Transform;
	}
	
	public Vector3 ConstrainLengthBackwards(Vector3 newPosition, float elasticity) {
		if (child == null) {
			return newPosition;
		}
		Vector3 diff = newPosition - child.position;
		Vector3 dir = diff.Normalized();
		return newPosition.Lerp(child.position + dir * child.GetLengthToParent(), elasticity);
	}
	
	public Vector3 ConstrainLength(Vector3 newPosition, float elasticity) {
		Vector3 diff = newPosition - parent.position;
		Vector3 dir = diff.Normalized();
		return newPosition.Lerp(parent.position + dir * GetLengthToParent(), elasticity);
	}

	/*public void PrepareTeleport() {
		if (!boneID.HasValue) {
			Vector3 parentTransformPosition = parent.GlobalTransform.Origin;
			preTeleportPosition = parent.GlobalTransform * (parent.ParentGlobalTransform.Inverse() * parentTransformPosition);
			return;
		}
		preTeleportPosition = GlobalTransform.Origin;
	}
	
	public void FinishTeleport() {
		Vector3 teleportedPosition;
		if (!boneID.HasValue) {
			Vector3 parentTransformPosition = parent.GlobalTransform.Origin;
			teleportedPosition = parent.GlobalTransform * parent.ParentGlobalTransform.Inverse() * parentTransformPosition;
		} else {
			teleportedPosition = GlobalTransform.Origin;
		}
		Vector3 diff = teleportedPosition - preTeleportPosition;
		lastTargetAnimatedBoneFrame = new PositionFrame(lastTargetAnimatedBoneFrame.position + diff, lastTargetAnimatedBoneFrame.time);
		currentTargetAnimatedBoneFrame = new PositionFrame(currentTargetAnimatedBoneFrame.position + diff, currentTargetAnimatedBoneFrame.time);
		position += diff;
		previousPosition += diff;
	}

	private Vector3 ConstrainAngleBackward(Vector3 newPosition, float elasticity, float elasticitySoften) {
		if (child == null || child.child == null) {
			return newPosition;
		}
		Vector3 cToDTargetPose = child.child.currentFixedAnimatedBonePosition - child.currentFixedAnimatedBonePosition;
		Vector3 cToD = child.child.position - child.position;
		Quaternion neededRotation = new Quaternion(cToDTargetPose.Normalized(), cToD.Normalized());
		Vector3 cToB = newPosition - child.position;
		Vector3 constraintTarget = neededRotation * cToB;

		
		//Debug.DrawLine(newPosition, child.position + constraintTarget, Color.cyan);
		float error = newPosition.DistanceTo(child.position + constraintTarget);
		error /= child.GetLengthToParent();
		error = Mathf.Clamp(error, 0f, 1f);
		error = Mathf.Pow(error, elasticitySoften * 2f);
		return newPosition.Lerp( child.position + constraintTarget, elasticity * error);
	}*/

	private Vector3 ConstrainAngle(Vector3 newPosition, float elasticity, float elasticitySoften) {
		Vector3 poseParentParent;
		Vector3 parentParentPosition;
		if (parent.parent == null) {
			poseParentParent = parent.currentFixedAnimatedBonePosition + (parent.currentFixedAnimatedBonePosition - currentFixedAnimatedBonePosition);
			parentParentPosition = poseParentParent;
		} else {
			parentParentPosition = parent.parent.position;
			poseParentParent = parent.parent.currentFixedAnimatedBonePosition;
		}
		Vector3 parentAimTargetPose = parent.currentFixedAnimatedBonePosition - poseParentParent;
		Vector3 parentAim = parent.position - parentParentPosition;
		DebugDraw3D.DrawLine(parent.position, parentParentPosition, Colors.Green);
		DebugDraw3D.DrawLine(poseParentParent, parent.currentFixedAnimatedBonePosition, Colors.Yellow);
		Quaternion targetPoseToPose = new Quaternion(parentAimTargetPose.Normalized(),parentAim.Normalized()).Normalized();
		Vector3 currentPose = currentFixedAnimatedBonePosition - poseParentParent;
		Vector3 constraintTarget = targetPoseToPose * currentPose;
		float error = newPosition.DistanceTo(parentParentPosition + constraintTarget);
		error /= GetLengthToParent();
		error = Mathf.Clamp(error, 0f, 1f);
		error = Mathf.Pow(error, elasticitySoften * 2f);
		return newPosition.Lerp(parentParentPosition + constraintTarget, elasticity * error);
	}

	private void RecordPosition(ulong time) {
		previousUpdateTime = updateTime;
		previousPosition = position;
		updateTime = time;
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
		double t = ((Time.GetTicksUsec() - smoothAmount) - previousUpdateTime) / (double)deltaTime;
		extrapolatedPosition = offset+previousPosition.Lerp(position, (float)t);
		return extrapolatedPosition;
	}

	public void PrepareBone() {
		// If bone is not animated, return to last unadulterated pose
		if (boneID.HasValue) {
			boneTransformChangeCheck = Transform;
		}
		CacheAnimationPosition();
	}

	public void OnDrawGizmos(JiggleSettingsBase jiggleSettings) {
		/*if (transform != null && child != null && child.transform != null) {
			Gizmos.DrawLine(transform.position, child.transform.position);
		}
		if (transform != null && child != null && child.transform == null) {
			Gizmos.DrawLine(transform.position, child.GetProjectedPosition());
		}
		if (transform != null && jiggleSettings != null) {
			Gizmos.DrawWireSphere(transform.position, jiggleSettings.GetRadius(normalizedIndex));
		}
		if (transform == null && jiggleSettings != null) {
			Gizmos.DrawWireSphere(GetProjectedPosition(), jiggleSettings.GetRadius(normalizedIndex));
		}*/
	}

	public void DrawDebug(float simulated) {
		Vector3 positionBlend = currentTargetAnimatedBoneFrame.position.Lerp(extrapolatedPosition, simulated);
		if (child != null) {
			Vector3 childPositionBlend = child.currentTargetAnimatedBoneFrame.position.Lerp(child.extrapolatedPosition, simulated);
			DebugDraw3D.DrawLine(positionBlend, childPositionBlend, Colors.Yellow);
		}
		if (boneID.HasValue) {
			DebugDraw3D.DrawLine(GlobalTransform.Origin, ParentGlobalTransform.Origin, Colors.Green);
		}
	}

	public void PoseBone(float blend) {
		if (child != null) {
			Vector3 positionBlend = currentTargetAnimatedBoneFrame.position.Lerp(extrapolatedPosition, blend);
			Vector3 childPositionBlend = child.currentTargetAnimatedBoneFrame.position.Lerp(child.extrapolatedPosition, blend);

			var inverseParent = (targetSkeleton.GlobalTransform.Inverse() * ParentGlobalTransform).Inverse();

			if (parent != null) {
				targetSkeleton.SetBonePosePosition(boneID??-1, inverseParent * positionBlend);
			}

			Vector3 childPosition;
			if (!child.boneID.HasValue) {
				childPosition = GlobalTransform * (ParentGlobalTransform.Inverse() * GlobalTransform.Origin);
			} else {
				childPosition = child.GlobalTransform.Origin;
			}

			Vector3 cachedAnimatedVector = childPosition - GlobalTransform.Origin;
			Vector3 simulatedVector = childPositionBlend - positionBlend;
			var localAnimatedVector = inverseParent.Basis * cachedAnimatedVector;
			var localSimulatedVector = inverseParent.Basis * simulatedVector;
			Quaternion animPoseToPhysicsPose = new Quaternion(localAnimatedVector.Normalized(), localSimulatedVector.Normalized());
			targetSkeleton.SetBonePoseRotation(boneID.Value,animPoseToPhysicsPose*targetSkeleton.GetBonePoseRotation(boneID.Value));
		}
		if (boneID.HasValue) {
			boneTransformChangeCheck = targetSkeleton.GetBoneGlobalPose(boneID.Value);
		}
	}
}