using Godot;
using System;

public partial class SmoothMover : Node3D {
	private Vector3 _position;
	public override void _Ready() {
		_position = Position;
	}
	public override void _Process(double delta) {
		Position = _position + Vector3.Forward * Mathf.Sin((Time.GetTicksUsec() / (float)1000000) * 5f)*1f;
	}
}
