using System.Collections;
using System.Collections.Generic;
using Godot;

// This is used to blend other jiggle settings together.
[GlobalClass]
[Tool]
public partial class JiggleSettingsBlend : JiggleSettingsBase {
    //[Tooltip("The list of jiggle settings to blend between.")]
    [Export]
    public Godot.Collections.Array<JiggleSettingsBase> blendSettings;
    //[Range(0f,1f)][Tooltip("A value from 0 to 1 that linearly blends between all of the blendSettings.")]
    [Export(PropertyHint.Range, "0,1")]
    public float normalizedBlend;
    public override float GetParameter(JiggleSettingParameter parameter) {
        int settingsCountSpace = blendSettings.Count - 1;
        float normalizedBlendClamp = Mathf.Clamp(normalizedBlend, 0f, 1f);
        int targetA = Mathf.Clamp(Mathf.FloorToInt(normalizedBlendClamp*settingsCountSpace), 0,settingsCountSpace);
        int targetB = Mathf.Clamp(Mathf.FloorToInt(normalizedBlendClamp*settingsCountSpace)+1, 0,settingsCountSpace);
        return Mathf.Lerp(
            blendSettings[targetA].GetParameter(parameter),
            blendSettings[targetB].GetParameter(parameter), 
            Mathf.Clamp(normalizedBlendClamp*settingsCountSpace-targetA, 0f, 1f)
            );
    }

    public override float GetRadius(float normalizedIndex) {
        float normalizedBlendClamp = Mathf.Clamp(normalizedBlend, 0f, 1f);
        int targetA = Mathf.FloorToInt(normalizedBlendClamp*blendSettings.Count);
        int targetB = Mathf.FloorToInt(normalizedBlendClamp*blendSettings.Count)+1;
        return Mathf.Lerp(blendSettings[Mathf.Clamp(targetA,0,blendSettings.Count-1)].GetRadius(normalizedIndex),
                          blendSettings[Mathf.Clamp(targetB,0,blendSettings.Count-1)].GetRadius(normalizedIndex), Mathf.Clamp(normalizedBlendClamp*blendSettings.Count-targetA, 0f, 1f));
    }
}
