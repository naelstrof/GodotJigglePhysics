namespace com.naelstrof.camera; 
using Godot;

[GlobalClass]
partial class CameraConfigurationBasic : CameraConfiguration {
	[Export] private CameraControl control;
	[Export] private CameraPivot pivot;
	public override void OnStartCameraFeed() {
		control.OnStartControlFeed();
	}

	public override void OnEndCameraFeed() {
		control.OnEndControlFeed();
	}

	public override CameraData GetCameraData() {
		return pivot.GetCameraData();
	}
}
