namespace com.naelstrof.camera; 
using Godot;
public struct CameraData {
    public Vector3 pivotPosition;
    public float yaw;
    public float pitch;
    public float distance;
    public float fov;
    public Vector2 screenOffset;

    public CameraData Lerp(CameraData to, float t) {
        return new CameraData {
            pivotPosition = pivotPosition.Lerp(to.pivotPosition, t),
            yaw = Mathf.Lerp(yaw, to.yaw, t),
            pitch = Mathf.Lerp(pitch, to.pitch, t),
            distance = Mathf.Lerp(distance, to.distance, t),
            fov = Mathf.Lerp(fov, to.fov, t),
            screenOffset = screenOffset.Lerp(to.screenOffset, t),
        };
    }
}
