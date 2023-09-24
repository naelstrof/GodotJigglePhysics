using Godot;
using System;
using NUnit.Framework;

public static class Vector3Extensions {
    public static Vector3 GetOrthogonal(Vector3 v) {
        Vector3 other = Vector3.Up;
        if (Mathf.IsEqualApprox(Mathf.Abs(other.Dot(v)), 1f)) {
            return Vector3.Right.Cross(v).Normalized();
        }
        return other.Cross(v).Normalized();
    }

    public static Quaternion FromToRotation(this Vector3 from, Vector3 to) {
        float cosTheta = from.Dot(to);
        float k = Mathf.Sqrt(from.LengthSquared() * to.LengthSquared());
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (cosTheta / k == -1) {
            Vector3 ortho = GetOrthogonal(from);
            return new Quaternion(ortho.X, ortho.Y, ortho.Z, 0f);
        }
        Vector3 cross = from.Cross(to);
        return new Quaternion(cross.X, cross.Y, cross.Z, cosTheta + k).Normalized();
    }
}

