using Godot;
using System;
using com.naelstrof.camera;

public partial class CharacterAnimationTree : AnimationTree {
	private RigidBody3D body;
	private Node3D visuals;
	private float walkTransition;
	public override void _Ready() {
		base._Ready();
		body = GetNodeOrNull<CharacterController>("../../../..");
		visuals = GetNodeOrNull<Node3D>("../..");
		walkTransition = 0f;
	}

	public override void _Process(double delta) {
		base._Process(delta);
		float speed = body.LinearVelocity.Length();
		walkTransition = Mathf.MoveToward(walkTransition, speed > 0.1f ? 1f : 0f, (float)delta * 8f);
		Set("parameters/Speed/scale", speed*0.12f);
		Set("parameters/WalkBlend/blend_amount", walkTransition);
		if (CharacterController.GetWishDir().Length() != 0f) {
			visuals.Quaternion =
				visuals.Quaternion.Slerp(new Quaternion(Vector3.Up, Mathf.Pi + OrbitCamera.GetCameraAngle().Y),
					(float)delta * 2f);
		}
	}
}
