namespace com.naelstrof.camera; 
using Godot;

[GlobalClass]
public abstract partial class CameraConfiguration : Node {
	public abstract void OnStartCameraFeed();
	public abstract void OnEndCameraFeed();
	public abstract CameraData GetCameraData();
}
