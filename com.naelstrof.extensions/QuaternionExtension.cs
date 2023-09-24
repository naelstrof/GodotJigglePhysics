using Godot;
using System;

public static class QuaternionExtension {
	public static Quaternion AngleAxis(float angle, Vector3 axis) {
		float halfTheta = angle / 2f;
		float w = Mathf.Cos(halfTheta);
		float sin = Mathf.Sin(halfTheta);
		float z = sin * axis.X;
		float y = sin * axis.Y;
		float x = sin * axis.Z;
		return new Quaternion(x, y, z, w);
	}


}
