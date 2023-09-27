using Godot;
using System.Collections.Generic;
using System.Linq;
using JigglePhysics;

[GlobalClass]
public partial class JiggleSkeleton : Skeleton3D {
	private List<JiggleRig> _rigs;
	private JiggleTimer _solveTimer;
	[Export]
	public AnimationPlayer.AnimationProcessCallback PlaybackProcessMode = AnimationPlayer.AnimationProcessCallback.Idle;
	public override void _Ready() {
		base._Ready();
		_solveTimer = new JiggleTimer(Time.GetTicksUsec(),60f, false);
		_solveTimer.Tick += ComputeSolve;
		
		_rigs = new List<JiggleRig>(GetChildren().OfType<JiggleRig>());
		foreach (var rig in _rigs) {
			rig.Initialize(this);
		}

		int beforeAnimation = 100;
		ProcessPriority = beforeAnimation;
		ProcessPhysicsPriority = beforeAnimation;
	}

	private void Sample(ulong time, ulong delta) {
		foreach(JiggleRig rig in _rigs) {
			rig.SampleBone(time);
		}
	}

	private void ComputeSolve(ulong time, ulong delta) {
		foreach(JiggleRig rig in _rigs) {
			Vector3 wind = Vector3.Zero;
			rig.FirstPass(wind, time, delta);
		}
		foreach (JiggleRig rig in _rigs) {
			rig.SecondPass(time);
		}
		foreach (JiggleRig rig in _rigs) {
			rig.ThirdPass(time);
		}
	}
	
	private void Pose() {
		ulong sampleTime = Time.GetTicksUsec();
		Sample(sampleTime, 0);
		_solveTimer.Update(sampleTime);
		foreach (JiggleRig rig in _rigs) {
			rig.DeriveFinalSolve(sampleTime);
		}
		foreach (JiggleRig rig in _rigs) {
			rig.Pose(sampleTime);
		}
	}

	private void Reset() {
		foreach (JiggleRig rig in _rigs) {
			rig.Reset();
		}
	}

	public override void _Process(double delta) {
		base._Process(delta);
		if (PlaybackProcessMode != AnimationPlayer.AnimationProcessCallback.Idle) return;
		Reset();
		CallDeferred(nameof(Pose));
	}

	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);
		if (PlaybackProcessMode != AnimationPlayer.AnimationProcessCallback.Physics) return;
		Reset();
		CallDeferred(nameof(Pose));
	}
}
