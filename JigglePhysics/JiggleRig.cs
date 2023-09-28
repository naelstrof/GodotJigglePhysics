using Godot;
using System;
using System.Collections.Generic;
using System.Text;
using JigglePhysics;
using Skeleton3D = Godot.Skeleton3D;

[GlobalClass,Tool]
public partial class JiggleRig : Node {
	private string _boneName;
	private string BoneName {
		get => _boneName;
		set {
			_boneName = value;
			NotifyPropertyListChanged();
		}
	}

	private int _boneId = -1;
	private int GetBone() {
		Skeleton3D skeleton = GetParentOrNull<Skeleton3D>();
		if (skeleton == null) {
			throw new Exception("Parent of jiggle rig wasn't a skeleton!");
		}
		for (int i = 0; i < skeleton.GetBoneCount(); i++) {
			if (skeleton.GetBoneName(i) == BoneName.Trim()) {
				_boneId = i;
				return i;
			}
		}
		throw new Exception($"Failed to find bone with name {BoneName}");
	}
	[Export]
	private JiggleSettingsBase _jiggleSettings;
	private List<JiggleBone> _simulatedPoints;

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
			{ "name", nameof(BoneName) },
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
			if (newJiggleBone.Parent == null) {
				var parentPosition = newJiggleBone.ParentGlobalTransform.Origin;
				float lengthToParent = currentTransformPosition.DistanceTo(parentPosition);
				Vector3 projectedForwardReal = (currentTransformPosition - parentPosition).Normalized();
				outputPoints.Add(new JiggleBone(targetSkeleton,null, newJiggleBone, currentTransformPosition + projectedForwardReal*lengthToParent));
				return;
			}
			Vector3 projectedForward = (currentTransformPosition - parentJiggleBone.GlobalTransform.Origin).Normalized();
			float length = 0.1f;
			if (parentJiggleBone.Parent != null) {
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
		_simulatedPoints = new List<JiggleBone>();
		CreateSimulatedPoints(targetSkeleton, _simulatedPoints, new int[]{}, GetBone(), null);
	}
	public void SampleBone(ulong time) {
		foreach (JiggleBone simulatedPoint in _simulatedPoints) {
			simulatedPoint.SampleBone(time);
		}
	}

	public void FirstPass(Vector3 wind, ulong time, ulong delta) {
		var data = _jiggleSettings.GetData();
		foreach (JiggleBone simulatedPoint in _simulatedPoints) {
			simulatedPoint.FirstPass(data, wind, time, delta);
		}
	}

	public void SecondPass(ulong time) {
		var data = _jiggleSettings.GetData();
		for (int i=_simulatedPoints.Count-1;i>=0;i--) {
			_simulatedPoints[i].SecondPass(data, time);
		}
	}

	public void ThirdPass(ulong time) {
		var data = _jiggleSettings.GetData();
		foreach (JiggleBone simulatedPoint in _simulatedPoints) {
			simulatedPoint.ThirdPass(data, time);
		}
	}

	public void DeriveFinalSolve(ulong time) {
		foreach (JiggleBone simulatedPoint in _simulatedPoints) {
			simulatedPoint.DeriveFinalSolvePosition(time);
		}
	}

	public void Pose(ulong time) {
		var data = _jiggleSettings.GetData();
		foreach (JiggleBone simulatedPoint in _simulatedPoints) {
			simulatedPoint.PoseBone(data.Blend, time);
		} 
	}

	public void Reset() {
		foreach (JiggleBone simulatedPoint in _simulatedPoints) {
			simulatedPoint.ResetBone();
		} 
	}

}
