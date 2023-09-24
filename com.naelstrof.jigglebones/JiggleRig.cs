using Godot;
using System;
using System.Collections.Generic;
using System.Text;
using JigglePhysics;
using Skeleton3D = Godot.Skeleton3D;

[GlobalClass,Tool]
public partial class JiggleRig : Node {
	private string _boneName;
	private string boneName {
		get => _boneName;
		set {
			_boneName = value;
			NotifyPropertyListChanged();
		}
	}

	private int _boneID = -1;
	private int GetBone() {
		if (_boneID != -1) {
			return _boneID;
		}
		Skeleton3D skeleton = GetParentOrNull<Skeleton3D>();
		if (skeleton == null) {
			throw new Exception("Parent of jiggle rig wasn't a skeleton!");
		}
		for (int i = 0; i < skeleton.GetBoneCount(); i++) {
			if (skeleton.GetBoneName(i) == boneName.Trim()) {
				_boneID = i;
				return i;
			}
		}
		throw new Exception($"Failed to find bone with name {boneName}");
	}
	[Export]
	private JiggleSettingsBase jiggleSettings;
	private List<JiggleBone> simulatedPoints;

	public override string[] _GetConfigurationWarnings() {
		Skeleton3D skeleton = GetParentOrNull<Skeleton3D>();
		if (skeleton == null || skeleton.IsClass("JiggleSkeleton")) {
			List<string> warnings = new List<string>(base._GetConfigurationWarnings()??Array.Empty<string>());
			if (skeleton == null) {
				warnings.Add("Parent must be a JiggleSkeleton!");
			} else if (!skeleton.IsClass("JiggleSkeleton")) {
				warnings.Add("Parent cannot simply be a Skeleton3D, please add the JiggleSkeleton script to it.");
			}
			return warnings.ToArray();
		}
		return base._GetConfigurationWarnings();
	}

	public override Godot.Collections.Array<Godot.Collections.Dictionary> _GetPropertyList() {
		var properties = new Godot.Collections.Array<Godot.Collections.Dictionary>();
		StringBuilder builder = new StringBuilder();
		
		Skeleton3D skeleton = GetParentOrNull<Skeleton3D>();
		if (skeleton != null) {
			for (int i = 0; i < skeleton.GetBoneCount(); i++) {
				for (int o = i; o > -1; o = skeleton.GetBoneParent(o)) {
					builder.Append(' ');
				}
				builder.Append(skeleton.GetBoneName(i));
				if (i != skeleton.GetBoneCount()) {
					builder.Append(',');
				}
			}
		} else {
			builder.Append("None");
		}
		properties.Add(new Godot.Collections.Dictionary {
			{ "name", "boneName" },
			{ "type", (int)Variant.Type.String },
			{ "usage", (int)PropertyUsageFlags.Default },
			{ "hint", (int)PropertyHint.Enum },
			{ "hint_string", builder.ToString() }
		});
		return properties;
	}
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
	    simulatedPoints = new List<JiggleBone>();
		CreateSimulatedPoints(targetSkeleton, simulatedPoints, new int[]{}, GetBone(), null);
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

    public void PrepareTeleport() {
	    foreach (JiggleBone simulatedPoint in simulatedPoints) {
		    simulatedPoint.PrepareTeleport();
	    }
    }

    public void FinishTeleport() {
	    foreach (JiggleBone simulatedPoint in simulatedPoints) {
		    simulatedPoint.FinishTeleport();
	    }
    }
}
