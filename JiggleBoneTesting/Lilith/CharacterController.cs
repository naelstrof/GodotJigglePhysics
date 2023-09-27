using Godot;
using System;
using com.naelstrof.camera;

[GlobalClass]
public partial class CharacterController : RigidBody3D {
	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);
		Vector3 inputDir = Vector3.Zero;
		if (Input.IsKeyPressed(Key.W)) {
			inputDir.Z -= 1;
		}
		if (Input.IsKeyPressed(Key.S)) {
			inputDir.Z += 1;
		}
		if (Input.IsKeyPressed(Key.D)) {
			inputDir.X += 1;
		}
		if (Input.IsKeyPressed(Key.A)) {
			inputDir.X -= 1;
		}
		
		Vector3 wishDir = new Quaternion(Vector3.Up,OrbitCamera.GetCameraAngle().Y) * inputDir;
		Vector3 velocity = LinearVelocity;
		velocity = Friction(velocity, 9f, (float)delta);
		velocity = Accelerate(velocity, wishDir, 8f, 11f, true, 0.1f, (float)delta);
		LinearVelocity = velocity;
	}

	private Vector3 Friction(Vector3 velocity, float effectiveFriction, float deltaTime) {
		float speed = velocity.Length();
		if ( speed < 0.1f ) {
			return Vector3.Zero;
		}
		float stopSpeed = 1f;
		float control = speed < stopSpeed ? stopSpeed : speed;
		float drop = 0;
		drop += control * effectiveFriction * deltaTime;
		float newspeed = speed - drop;
		if (newspeed < 0) {
			newspeed = 0;
		}
		newspeed /= speed;
		return velocity * newspeed;
	}
	
	private Vector3 Accelerate(Vector3 velocity, Vector3 wishdir, float wishspeed, float accel, bool grounded, float airCap, float deltaTime) {
		float wishspd = wishspeed;
		if (!grounded) {
			wishspd = Mathf.Min(wishspd, airCap);
		}
		float currentspeed = velocity.Dot(wishdir);

		float addspeed = wishspd - currentspeed;
		if (addspeed <= 0) {
			return velocity;
		}
		float accelspeed = accel * wishspeed * deltaTime;

		accelspeed = Mathf.Min(accelspeed, addspeed);

		return velocity + accelspeed * wishdir;
	}
}
