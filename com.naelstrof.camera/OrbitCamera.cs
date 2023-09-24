namespace com.naelstrof.camera; 
using Godot;

[GlobalClass]
partial class OrbitCamera : Camera3D {
    [Export] private CameraConfiguration _startingConfiguration;
    
    private CameraData _data;
    public override void _Ready() {
        base._Ready();
        _data = _startingConfiguration.GetCameraData();
        _startingConfiguration.OnStartCameraFeed();
    }

    public override void _ExitTree() {
        base._ExitTree();
        _startingConfiguration.OnEndCameraFeed();
    }

    public override void _Process(double delta) {
        base._Process(delta);
        _data = _startingConfiguration.GetCameraData();
        GlobalRotation = new Vector3(_data.pitch, _data.yaw, 0f);
        var viewport = GetViewport();
        Vector2 realScreenPosition = new Vector2(
            viewport.GetVisibleRect().Size.X * _data.screenOffset.X,
            viewport.GetVisibleRect().Size.Y * _data.screenOffset.Y
        );
        Vector3 ray = ProjectRayNormal(realScreenPosition);
        GlobalPosition = _data.pivotPosition - ray * _data.distance;
    }
}