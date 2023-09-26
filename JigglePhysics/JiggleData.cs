using Godot;

namespace JigglePhysics; 

public struct JiggleData {
    public float GravityMultiplier;
    public float Friction;
    public float AirFriction;
    public float Blend;
    public float AngleElasticity;
    public float ElasticitySoften;
    public float LengthElasticity;

    public JiggleData Lerp(JiggleData to, float t) {
        return new JiggleData {
            GravityMultiplier = Mathf.Lerp(GravityMultiplier, to.GravityMultiplier, t),
            Friction = Mathf.Lerp(Friction, to.Friction, t),
            AirFriction = Mathf.Lerp(AirFriction, to.AirFriction, t),
            Blend = Mathf.Lerp(Blend, to.Blend, t),
            AngleElasticity = Mathf.Lerp(AngleElasticity, to.AngleElasticity, t),
            ElasticitySoften = Mathf.Lerp(ElasticitySoften, to.ElasticitySoften, t),
            LengthElasticity = Mathf.Lerp(LengthElasticity, to.LengthElasticity, t),
        };
    }
}