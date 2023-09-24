using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

[GlobalClass]
public partial class JiggleSkeleton : Skeleton3D {
	[Export]
	private AnimationPlayer player;
	[Export]
	private AnimationPlayer.AnimationProcessCallback PlaybackProcessMode = AnimationPlayer.AnimationProcessCallback.Idle;
	
	private List<JiggleRig> rigs;
	private ulong lastUpdate;
	private ulong accumulation;
	// 60 hz
	private const ulong fixedTickDelta = 16667;
	
	public override void _Ready() {
		base._Ready();
		player.PlaybackProcessMode = AnimationPlayer.AnimationProcessCallback.Manual;
		rigs = new List<JiggleRig>(GetChildren().OfType<JiggleRig>());
		foreach (var rig in rigs) {
			rig.Initialize(this);
		}
		lastUpdate = Time.GetTicksUsec();
	}

	public void Advance(double delta) {
		foreach(JiggleRig rig in rigs) {
			rig.Reset();
		}
		player.Advance(delta);
		foreach(JiggleRig rig in rigs) {
			rig.PrepareBone();
		}
		ulong udelta = Time.GetTicksUsec() - lastUpdate;
		lastUpdate = Time.GetTicksUsec();
		const ulong secondsToMicroseconds = 1000000;
		const float fixedFloatDelta = (float)fixedTickDelta / (float)secondsToMicroseconds;
		accumulation += udelta;
		while (accumulation > fixedTickDelta) {
			accumulation -= fixedTickDelta;
			ulong time = Time.GetTicksUsec() - fixedTickDelta;
			foreach(JiggleRig rig in rigs) {
				Vector3 wind = Vector3.Zero;
				rig.FirstPass(wind, time, fixedFloatDelta);
			}
			foreach (JiggleRig rig in rigs) {
				rig.SecondPass();
			}
			foreach (JiggleRig rig in rigs) {
				rig.ThirdPass();
			}
			//foreach (JiggleRig rig in rigs) {
				//rig.FinalPass(time);
			//}
		}
		foreach (JiggleRig rig in rigs) {
			rig.DeriveFinalSolve(fixedTickDelta);
		}
		foreach (JiggleRig rig in rigs) {
			rig.Pose();
		}
	}

	public override void _Process(double delta) {
		base._Process(delta);
		if (PlaybackProcessMode != AnimationPlayer.AnimationProcessCallback.Idle) {
			return;
		}
		Advance(delta);
	}

	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);
		if (PlaybackProcessMode != AnimationPlayer.AnimationProcessCallback.Physics) {
			return;
		}
		Advance(delta);
	}
}
