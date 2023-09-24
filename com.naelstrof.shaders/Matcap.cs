using Godot;
using System;

[Tool]
public partial class Matcap : VisualShaderNodeCustom {
	
	public override string _GetName() {
		return "Matcap";
	}

	public override string _GetCategory() {
		return "Custom";
	}

	public override string _GetDescription() {
		return "Samples a texture as a Matcap based on the provided world normal";
	}

	public override PortType _GetReturnIconType() {
		return PortType.Vector4D;
	}

	public override int _GetInputPortCount() {
		return 2;
	}

	public override string _GetInputPortName(int i) {
		return i switch {
			0 => "sampler2D",
			1 => "world_normal",
			_ => throw new ArgumentOutOfRangeException(nameof(i), i, $"No port with index {i}")
		};
	}

	public override PortType _GetInputPortType(int i) {
		return i switch {
			0 => PortType.Sampler,
			1 => PortType.Vector3D,
			_ => throw new ArgumentOutOfRangeException(nameof(i), i, $"No port with index {i}")
		};
	}

	public override int _GetOutputPortCount() {
		return 1;
	}

	public override string _GetOutputPortName(int i) {
		return "color";
	}

	public override string _GetGlobalCode(Shader.Mode mode) {
		return @"
void GetOrthoNormalized(inout vec3 a, inout vec3 b, out vec3 c) {
	a = normalize(a);
	c = normalize(cross(a, b));
	b = cross(c, a);
}
vec4 GetMatCap(sampler2D mat, vec3 vertToView, vec3 viewUp, vec3 worldNormal) {
	vec3 c;
	GetOrthoNormalized(vertToView, viewUp, c);
	float dotA = dot(viewUp, worldNormal);
	float dotB = dot(c, worldNormal);
	vec2 uv = vec2(0.5,0.5)+vec2(0.5,0.5)*vec2(dotA,dotB);
	return texture(mat,uv);
}
";
	}

	public override PortType _GetOutputPortType(int i) {
		return PortType.Vector4D;
	}

	public override string _GetCode(Godot.Collections.Array<string> inputVars, Godot.Collections.Array<string> outputVars, Shader.Mode mode, VisualShader.Type type) {
		return $"{outputVars[0]} = GetMatCap({inputVars[0]},-VIEW, (VIEW_MATRIX*vec4(0,1,0,0)).xyz, {inputVars[1]});";
	}
}
