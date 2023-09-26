using Godot;
using System.Collections.Generic;
using System.Linq;

[GlobalClass]
public partial class JiggleSkeleton : Skeleton3D {
	private List<JiggleRig> _rigs;
	private ulong _lastUpdate;
	private ulong _accumulation;
	// 60 hz
	[Export]
	public AnimationPlayer.AnimationProcessCallback PlaybackProcessMode = AnimationPlayer.AnimationProcessCallback.Idle;
	private const ulong FixedTickDelta = 16667;
	public override void _Ready() {
		base._Ready();
		_rigs = new List<JiggleRig>(GetChildren().OfType<JiggleRig>());
		foreach (var rig in _rigs) {
			rig.Initialize(this);
		}

		int beforeAnimation = -1000;
		ProcessPriority = beforeAnimation;
		ProcessPhysicsPriority = beforeAnimation;
		_lastUpdate = Time.GetTicksUsec();
	}

	public void Advance() {
		foreach(JiggleRig rig in _rigs) {
			rig.PrepareBone();
		}
		ulong uTicksDelta = Time.GetTicksUsec() - _lastUpdate;
		_lastUpdate = Time.GetTicksUsec();
		const ulong secondsToMicroseconds = 1000000;
		const float fixedFloatDelta = (float)FixedTickDelta / (float)secondsToMicroseconds;
		_accumulation += uTicksDelta;
		while (_accumulation > FixedTickDelta) {
			_accumulation -= FixedTickDelta;
			ulong time = Time.GetTicksUsec() - FixedTickDelta;
			foreach(JiggleRig rig in _rigs) {
				Vector3 wind = Vector3.Zero;
				rig.FirstPass(wind, time, fixedFloatDelta);
			}
			foreach (JiggleRig rig in _rigs) {
				rig.SecondPass();
			}
			foreach (JiggleRig rig in _rigs) {
				rig.ThirdPass();
			}
		}
		foreach (JiggleRig rig in _rigs) {
			rig.DeriveFinalSolve(FixedTickDelta);
		}
		foreach (JiggleRig rig in _rigs) {
			rig.Pose();
		}
	}

	public void Reset() {
		foreach (JiggleRig rig in _rigs) {
			rig.Reset();
		}
	}

	public override void _Process(double delta) {
		base._Process(delta);
		if (PlaybackProcessMode != AnimationPlayer.AnimationProcessCallback.Idle) return;
		Reset();
		CallDeferred(nameof(Advance));
	}

	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);
		if (PlaybackProcessMode != AnimationPlayer.AnimationProcessCallback.Physics) return;
		Reset();
		CallDeferred(nameof(Advance));
	}
}
