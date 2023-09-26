using Godot;
using JigglePhysics;

// This is used to blend other jiggle settings together.
[GlobalClass, Tool]
public partial class JiggleSettingsBlend : JiggleSettingsBase {
    //[Tooltip("The list of jiggle settings to blend between.")]
    [Export]
    public Godot.Collections.Array<JiggleSettingsBase> BlendSettings;
    //[Range(0f,1f)][Tooltip("A value from 0 to 1 that linearly blends between all of the blendSettings.")]
    [Export(PropertyHint.Range, "0,1")]
    public float NormalizedBlend;

    public override JiggleData GetData() {
        int settingsCountSpace = BlendSettings.Count - 1;
        float normalizedBlendClamp = Mathf.Clamp(NormalizedBlend, 0f, 1f);
        int targetA = Mathf.Clamp(Mathf.FloorToInt(normalizedBlendClamp*settingsCountSpace), 0,settingsCountSpace);
        int targetB = Mathf.Clamp(Mathf.FloorToInt(normalizedBlendClamp*settingsCountSpace)+1, 0,settingsCountSpace);
        return BlendSettings[targetA].GetData().Lerp(BlendSettings[targetB].GetData(),
            Mathf.Clamp(normalizedBlendClamp*settingsCountSpace-targetA, 0f, 1f)
            );
    }
}
