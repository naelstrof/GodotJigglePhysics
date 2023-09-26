using System;
using Godot;
using JigglePhysics;

[GlobalClass, Tool]
public partial class JiggleSettings : JiggleSettingsBase {
    [Export(PropertyHint.Range, "0,2")]
    private float _gravityMultiplier = 1f;
    [Export(PropertyHint.Range, "0,1")]
    private float _friction = 0.2f;
    [Export(PropertyHint.Range, "0,1")]
    private float _angleElasticity = 0.4f;
    [Export(PropertyHint.Range, "0,1")]
    private float _blend = 1f;
    [Export(PropertyHint.Range, "0,1")]
    private float _airDrag = 0.1f;
    [Export(PropertyHint.Range, "0,1")]
    private float _lengthElasticity = 0.8f;
    [Export(PropertyHint.Range, "0,1")]
    private float _elasticitySoften = 0f;

    public override JiggleData GetData() {
        return new JiggleData {
            GravityMultiplier = _gravityMultiplier,
            Friction = _friction,
            AngleElasticity = _angleElasticity,
            Blend = _blend,
            AirFriction = _airDrag,
            LengthElasticity = _lengthElasticity,
            ElasticitySoften = _elasticitySoften,
        };
    }
}
