using Godot;
using System;

[GlobalClass]
public partial class GameManager : Node {
	public override void _UnhandledInput(InputEvent @event) {
		base._UnhandledInput(@event);
		if (@event is InputEventKey { Keycode: Key.Escape }) {
			GetTree().Quit();
		}
	}
}
