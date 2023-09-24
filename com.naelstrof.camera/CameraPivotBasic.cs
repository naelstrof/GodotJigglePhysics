namespace com.naelstrof.camera; 
using Godot;
using System;

[GlobalClass]
public partial class CameraPivotBasic : CameraPivot {
    [Export] private CameraControl cameraControl { get; set; }
    [Export] private float desiredDistance = 1f;
    [Export] private float desiredFOV=75f;
    [Export] private Vector2 desiredScreenOffset=new(0.5f,0.5f);
    public override void _Ready() {
        base._Ready();
        if (cameraControl == null) {
            throw new Exception("Pivot must have control assigned.");
        }
    }

    public override CameraData GetCameraData() {
        cameraControl.GetYawPitch(out float yaw, out float pitch);
        return new CameraData {
            pivotPosition = GlobalPosition,
            distance = desiredDistance,
            fov = desiredFOV,
            screenOffset = desiredScreenOffset,
            yaw = yaw,
            pitch = pitch,
        };
    }
}
