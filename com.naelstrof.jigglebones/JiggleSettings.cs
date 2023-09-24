using System;
using Godot;

[GlobalClass]
public partial class JiggleSettings : JiggleSettingsBase {
    //[SerializeField] [Range(0f,2f)] [Tooltip("How much gravity to apply to the simulation, it is a multiplier of the Physics.gravity setting.")]
    [Export(PropertyHint.Range, "0,2")]
    private float gravityMultiplier = 1f;
    //[SerializeField] [Range(0f,1f)] [Tooltip("How much mechanical friction to apply, this is specifically how quickly oscillations come to rest.")]
    [Export(PropertyHint.Range, "0,1")]
    private float friction = 0.4f;
    //[SerializeField] [Range(0f,1f)] [Tooltip("How much angular force is applied to bring it to the target shape.")]
    [Export(PropertyHint.Range, "0,1")]
    private float angleElasticity = 0.4f;
    //[SerializeField] [Range(0f,1f)] [Tooltip("How much of the simulation should be expressed. A value of 0 would make the jiggle have zero effect. A value of 1 gives the full movement as intended. 0.5 would ")]
    [Export(PropertyHint.Range, "0,1")]
    private float blend = 1f;
    //[FormerlySerializedAs("airFriction")] [HideInInspector] [SerializeField] [Range(0f, 1f)] [Tooltip( "How much jiggled objects should get dragged behind by moving through the air. Or how \"thick\" the air is.")]
    [Export(PropertyHint.Range, "0,1")]
    private float airDrag = 0.1f;
    //[HideInInspector] [SerializeField] [Range(0f, 1f)] [Tooltip( "How rigidly the rig holds its length. Low values cause lots of squash and stretch!")]
    [Export(PropertyHint.Range, "0,1")]
    private float lengthElasticity = 0.8f;
    //[HideInInspector] [SerializeField] [Range(0f, 1f)] [Tooltip("How much to allow free bone motion before engaging elasticity.")]
    [Export(PropertyHint.Range, "0,1")]
    private float elasticitySoften = 0f;
    //[HideInInspector] [SerializeField] [Tooltip("How much radius points have, only used for collisions. Set to 0 to disable collisions")]
    [Export]
    private float radiusMultiplier = 0f;
    //[HideInInspector] [SerializeField] [Tooltip("How the radius is expressed as a curve along the bone chain from root to child.")]
    [Export]
    private Curve radiusCurve  = new() {
        MinValue = 0f,
        MaxValue = 1f,
    };
    
    public override float GetParameter(JiggleSettingParameter parameter) {
        switch(parameter) {
            case JiggleSettingParameter.Gravity: return gravityMultiplier;
            case JiggleSettingParameter.Friction: return friction;
            case JiggleSettingParameter.AirFriction: return airDrag;
            case JiggleSettingParameter.Blend: return blend;
            case JiggleSettingParameter.AngleElasticity: return angleElasticity;
            case JiggleSettingParameter.ElasticitySoften: return elasticitySoften;
            case JiggleSettingParameter.LengthElasticity: return lengthElasticity;
            default: throw new Exception("Invalid Jiggle Setting Parameter:"+parameter);
        }
    }
    public void SetParameter(JiggleSettingParameter parameter, float value) {
        switch(parameter) {
            case JiggleSettingParameter.Gravity: gravityMultiplier = value; break;
            case JiggleSettingParameter.Friction: friction = value; break;
            case JiggleSettingParameter.AirFriction: airDrag = value; break;
            case JiggleSettingParameter.Blend: blend = value; break;
            case JiggleSettingParameter.AngleElasticity: angleElasticity = value; break;
            case JiggleSettingParameter.ElasticitySoften: elasticitySoften = value; break;
            case JiggleSettingParameter.LengthElasticity: lengthElasticity = value; break;
            default: throw new Exception("Invalid Jiggle Setting Parameter:"+parameter);
        }
    }

    public override float GetRadius(float normalizedIndex) {
        return radiusMultiplier * radiusCurve.Sample(normalizedIndex);
    }
}
