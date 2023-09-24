namespace com.naelstrof.camera; 
using Godot;

[GlobalClass]
public abstract partial class CameraPivot : Node3D {
    public abstract CameraData GetCameraData();
}