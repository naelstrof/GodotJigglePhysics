using System.Collections.Generic;
using Godot;

namespace JigglePhysics; 

public class JiggleTimer {
	public delegate void OnTickAction(ulong time, ulong delta);
	public event OnTickAction Tick;
	
	private readonly ulong _fixedTickDelta;
	private ulong _accumulation;
	private ulong _lastUpdate;
	private readonly bool _skip;
	public ulong GetFixedDelta() => _fixedTickDelta;
	public ulong GetTime() => _lastUpdate+_accumulation;
	public JiggleTimer(ulong currentTime, double hertz, bool skip) {
		double seconds = 1f / hertz;
		ulong microseconds = (ulong)(seconds * 1000000f);
		_fixedTickDelta = microseconds;
		_lastUpdate = currentTime;
		_skip = skip;
	}
	public void Update(ulong currentTime) {
		ulong delta = currentTime - _lastUpdate;
		_accumulation += delta;
		while (_accumulation >= _fixedTickDelta) {
			if (_skip) {
				_accumulation %= _fixedTickDelta;
			} else {
				_accumulation -= _fixedTickDelta;
			}
			ulong time = currentTime - _accumulation;
			Tick?.Invoke(time, delta);
		}
		_lastUpdate = currentTime;
	}
}