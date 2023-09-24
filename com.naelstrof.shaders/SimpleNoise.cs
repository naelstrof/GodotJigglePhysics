using Godot;
using System;

[Tool]
public partial class SimpleNoise : VisualShaderNodeCustom {
	
	public override string _GetName() {
		return "SimpleNoise";
	}

	public override string _GetCategory() {
		return "Custom";
	}
	
	public override string _GetDescription() {
		return "Gets procedural noise from an input seed.";
	}

	public override PortType _GetReturnIconType() {
		return PortType.Scalar;
	}

	public override int _GetInputPortCount() {
		return 1;
	}

	public override string _GetInputPortName(int i) {
		return "x";
	}

	public override PortType _GetInputPortType(int i) {
		return PortType.Scalar;
	}

	public override int _GetOutputPortCount() {
		return 1;
	}

	public override string _GetOutputPortName(int i) {
		return "noise";
	}

	public override PortType _GetOutputPortType(int i) {
		return PortType.Scalar;
	}

	public override string _GetGlobalCode(Shader.Mode mode) {
		return @"
float simpleRand(float n) {
	return fract(sin(n) * 43758.5453123);
}
float simpleNoise(float p) {
	float fl = floor(p);
	float fc = fract(p);
	return mix(simpleRand(fl), simpleRand(fl + 1.0), fc);
}";
	}

	public override string _GetCode(Godot.Collections.Array<string> inputVars, Godot.Collections.Array<string> outputVars, Shader.Mode mode, VisualShader.Type type) {
		return $"{outputVars[0]} = simpleNoise({inputVars[0]});";
	}
}
