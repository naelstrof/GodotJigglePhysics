using Godot;
using System;

public static class Node3DExtensions {
	public static Quaternion LocalQuaternion(this Node3D self) {
		var parent = self.GetParentNode3D();
		if (parent == null) {
			return Quaternion.Identity;
		}
		return parent.Quaternion.Inverse() * self.Quaternion;
	}

	public static void LocalQuaternion(this Node3D self, Quaternion newValue) {
		var parent = self.GetParentNode3D();
		if (parent == null) {
			self.Quaternion = newValue;
			return;
		}
		self.Quaternion = parent.Quaternion * self.Quaternion;
	}
}
