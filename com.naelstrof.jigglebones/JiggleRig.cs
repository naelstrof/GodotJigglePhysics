using Godot;
using System;
using System.Collections.Generic;
using JigglePhysics;
using Skeleton3D = Godot.Skeleton3D;

[GlobalClass]
public partial class JiggleRig : Node {

	[Export] private string rootBoneName;
	private int rootBoneID;
	[Export] private JiggleSettings jiggleSettings;
	private List<JiggleBone> simulatedPoints;
	private static void CreateSimulatedPoints(Skeleton3D targetSkeleton, ICollection<JiggleBone> outputPoints, ICollection<int> ignoredTransforms, int currentTransform, JiggleBone parentJiggleBone) {
        var currentTransformPosition = targetSkeleton.GetBonePosePosition(currentTransform);
        JiggleBone newJiggleBone = new JiggleBone(targetSkeleton, currentTransform, parentJiggleBone, currentTransformPosition);
        outputPoints.Add(newJiggleBone);
        // Create an extra purely virtual point if we have no children.
        var children = targetSkeleton.GetBoneChildren(currentTransform);
        if (children.Length == 0) {
            if (newJiggleBone.parent == null) {
	            var parentPosition = newJiggleBone.ParentGlobalTransform.Origin;
				float lengthToParent = currentTransformPosition.DistanceTo(parentPosition);
				Vector3 projectedForwardReal = (currentTransformPosition - parentPosition).Normalized();
				outputPoints.Add(new JiggleBone(targetSkeleton,null, newJiggleBone, currentTransformPosition + projectedForwardReal*lengthToParent));
				return;
            }
            Vector3 projectedForward = (currentTransformPosition - parentJiggleBone.GlobalTransform.Origin).Normalized();
            float length = 0.1f;
            if (parentJiggleBone.parent != null) {
                length = parentJiggleBone.GlobalTransform.Origin.DistanceTo(parentJiggleBone.ParentGlobalTransform.Origin);
            }
            outputPoints.Add(new JiggleBone(targetSkeleton, null, newJiggleBone, currentTransformPosition + projectedForward*length));
            return;
        }
        foreach(var child in children) {
            if (ignoredTransforms.Contains(child)) {
                continue;
            }
            CreateSimulatedPoints(targetSkeleton, outputPoints, ignoredTransforms, child, newJiggleBone);
        }
    }

	public void Initialize(Skeleton3D targetSkeleton) {
	    for (int i = 0; i < targetSkeleton.GetBoneCount(); i++) {
		    if (targetSkeleton.GetBoneName(i) != rootBoneName) {
			    continue;
		    }
		    rootBoneID = i;
		    break;
	    }
	    simulatedPoints = new List<JiggleBone>();
		CreateSimulatedPoints(targetSkeleton, simulatedPoints, new int[]{}, rootBoneID, null);
		foreach (var simulatedPoint in simulatedPoints) {
			simulatedPoint.CalculateNormalizedIndex();
		}
	}
    public void PrepareBone() {
	    foreach (JiggleBone simulatedPoint in simulatedPoints) {
		    simulatedPoint.PrepareBone();
	    }
    }

    public void FirstPass(Vector3 wind, ulong time, float deltaTime) {
	    foreach (JiggleBone simulatedPoint in simulatedPoints) {
		    simulatedPoint.FirstPass(jiggleSettings, wind, time, deltaTime);
	    }
    }

    public void SecondPass() {
	    for (int i=simulatedPoints.Count-1;i>=0;i--) {
		    simulatedPoints[i].SecondPass(jiggleSettings);
	    }
    }

    public void ThirdPass() {
	    foreach (JiggleBone simulatedPoint in simulatedPoints) {
		    simulatedPoint.ThirdPass(jiggleSettings);
	    }
    }

    //public void FinalPass(ulong time) {
	 //   foreach (JiggleBone simulatedPoint in simulatedPoints) {
	//	    simulatedPoint.final(jiggleSettings);
	 //   }
    //}

    public void DeriveFinalSolve(ulong deltaTime) {
	    Vector3 virtualPosition = simulatedPoints[0].DeriveFinalSolvePosition(Vector3.Zero, 1f, deltaTime);
	    Vector3 offset = simulatedPoints[0].position - virtualPosition;
	    foreach (JiggleBone simulatedPoint in simulatedPoints) {
		    simulatedPoint.DeriveFinalSolvePosition(offset, 1f, deltaTime);
	    }
    }

    public void Pose() {
	    foreach (JiggleBone simulatedPoint in simulatedPoints) {
		    simulatedPoint.PoseBone( jiggleSettings.GetParameter(JiggleSettings.JiggleSettingParameter.Blend));
	    } 
    }

    public void Reset() {
	    foreach (JiggleBone simulatedPoint in simulatedPoints) {
		    simulatedPoint.ResetBone();
	    } 
    }

    public void DebugDraw() {
	    foreach (JiggleBone simulatedPoint in simulatedPoints) {
		    simulatedPoint.DrawDebug(1f);
	    }
    }

    /*public void PrepareTeleport() {
	    foreach (JiggleBone simulatedPoint in simulatedPoints) {
		    simulatedPoint.PrepareTeleport();
	    }
    }

    public void FinishTeleport() {
	    foreach (JiggleBone simulatedPoint in simulatedPoints) {
		    simulatedPoint.FinishTeleport();
	    }
    }*/
}
