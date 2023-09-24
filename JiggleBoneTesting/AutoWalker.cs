using Godot;
using System;

[GlobalClass]
public partial class AutoWalker : AnimationPlayer {
	public override void _Ready() {
		base._Ready();
		Autoplay = "Walking/mixamo_com";
	}
}
