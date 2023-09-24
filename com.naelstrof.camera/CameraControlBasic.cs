namespace com.naelstrof.camera; 
using Godot;

[GlobalClass]
partial class CameraControlBasic : CameraControl {
    private Vector2 mousePos;
    private bool feedingControl;
    public override void GetYawPitch(out float yaw, out float pitch) {
        yaw = mousePos.X;
        pitch = mousePos.Y;
    }

    public override void OnStartControlFeed() {
        Input.MouseMode = Input.MouseModeEnum.Captured;
        feedingControl = true;
    }
    
    public override void OnEndControlFeed() {
        Input.MouseMode = Input.MouseModeEnum.Visible;
        feedingControl = false;
    }

    public override void _Input(InputEvent @event) {
        if (!feedingControl) {
            return;
        }
        if (@event is not InputEventMouseMotion mouseMotion) return;
        mousePos -= mouseMotion.Relative*0.01f;
        mousePos.X = Mathf.PosMod(mousePos.X, Mathf.Pi * 2f);
        mousePos.Y = Mathf.Clamp(mousePos.Y, -Mathf.Pi*0.5f, Mathf.Pi*0.5f);
    }
}