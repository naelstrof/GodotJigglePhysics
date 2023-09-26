using Godot;
namespace JigglePhysics; 

[GlobalClass, Tool]
public abstract partial class JiggleSettingsBase : Resource {
    public abstract JiggleData GetData();
}
