using Godot;
using System;

public partial class JiggleDebugPlotter : Control {
	private class LinePlot {
		private Line2D line;
		public LinePlot(Control target, Color color) {
			line = new Line2D();
			line.Width = 1;
			line.DefaultColor = color;
			target.AddChild(line);
		}

		public void AddPoint(float x, float y) {
			line.AddPoint(new Vector2(x,y));
		}

		public void SetVisible(bool enabled) {
			line.Visible = enabled;
		}

		public float GetWidth() {
			if (line.Points.Length == 0) {
				return 0f;
			}
			float min = float.MaxValue;
			float max = float.MinValue;
			foreach (var point in line.Points) {
				min = Mathf.Min(point.X, min);
				max = Mathf.Max(point.X, max);
			}
			return max - min;
		}
	}

	[Export]
	private JiggleRig targetRig;

	private LinePlot realPosition;
	private LinePlot predictedPosition;
	private LinePlot virtualPosition;
	private ulong startTime;
	public override void _Ready() {
		realPosition = new LinePlot(this, Colors.Red);
		predictedPosition = new LinePlot(this, Colors.Blue);
		virtualPosition = new LinePlot(this, Colors.Green);
		startTime = Time.GetTicksUsec();
	}

	private float MicrosecondsToReadable(ulong time) {
		if (time <= startTime) {
			return 0f;
		}
		return ((float)(time - startTime) / (float)100000)*400f;
	}

	public override void _Process(double delta) {
		/*var sampledPositionFrame = targetRig.GetSampledPosition();
		realPosition.AddPoint(MicrosecondsToReadable(sampledPositionFrame.Time), sampledPositionFrame.Position.Z*40+200);
		var predictedPositionFrame = targetRig.GetPredictedPosition();
		predictedPosition.AddPoint(MicrosecondsToReadable(predictedPositionFrame.Time), predictedPositionFrame.Position.Z*40+200);
		var virtualPositionFrame = targetRig.GetParticleFrame();
		virtualPosition.AddPoint(MicrosecondsToReadable(virtualPositionFrame.Time), virtualPositionFrame.Position.Z*40+200);
		float width = realPosition.GetWidth();
		width = Mathf.Max(width, predictedPosition.GetWidth());
		width = Mathf.Max(width, virtualPosition.GetWidth());
		CustomMinimumSize = CustomMinimumSize with { X = width };*/
	}
	private void _on_real_position_toggle_toggled(bool button_pressed) {
		realPosition.SetVisible(button_pressed);
	}


	private void _on_predicted_position_toggle_toggled(bool button_pressed) {
		predictedPosition.SetVisible(button_pressed);
	}


	private void _on_virtual_position_toggle_toggled(bool button_pressed) {
		virtualPosition.SetVisible(button_pressed);
	}
}


