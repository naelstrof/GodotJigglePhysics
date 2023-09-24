namespace com.naelstrof.camera; 
using Godot;

[GlobalClass]
public abstract partial class CameraControl : Node {
    public abstract void GetYawPitch(out float yaw, out float pitch);
    public abstract void OnStartControlFeed();
    public abstract void OnEndControlFeed();
}