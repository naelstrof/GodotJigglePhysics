use godot::prelude::*;


#[derive(GodotClass,Clone)]
#[class(init, base=Resource)]
pub struct JiggleSettingsData {
    #[init(default=1.0)]
    #[export]
    gravity: f32,
    #[init(default=0.1)]
    #[export]
    friction: f32,
    #[init(default=0.05)]
    #[export]
    air_friction: f32,
    #[init(default=1.0)]
    #[export]
    blend: f32,
    #[init(default=0.4)]
    #[export]
    angle_elasticity: f32,
    #[init(default=0.8)]
    #[export]
    length_elasticity: f32,
    #[init(default=0.0)]
    #[export]
    elasticity_soften: f32,
}

#[godot_api]
impl JiggleSettingsData {}
#[godot_api]
impl ResourceVirtual for JiggleSettingsData { }

fn get_jiggle_parameters(other : Option<Gd<Resource>>) -> Option<JiggleSettingsData> {
    if let Some(other) = Gd::try_cast::<JiggleSettingsData>(other?) {
        Some(other.bind().clone())
    } else {
        None
    }
}


impl JiggleSettingsData {
    pub fn lerp(&self, other : &Self, blend : f32) -> Self {
        Self {
            gravity: self.gravity.lerp(other.gravity, blend),
            friction: self.friction.lerp(other.friction, blend),
            air_friction: self.air_friction.lerp(other.air_friction, blend),
            blend: self.blend.lerp(other.blend, blend),
            angle_elasticity: self.angle_elasticity.lerp(other.angle_elasticity, blend),
            length_elasticity: self.length_elasticity.lerp(other.length_elasticity, blend),
            elasticity_soften: self.elasticity_soften.lerp(other.elasticity_soften, blend),
        }
    }
}

#[derive(GodotClass,Clone)]
#[class(base=Resource)]
struct JiggleSettingsBlend {
    #[export]
    settings: godot::builtin::Array<Option<Gd<Resource>>>,
    #[init(default=0.0)]
    #[export(range=(0.0,1.0))]
    blend: f32,
}

#[godot_api]
impl JiggleSettingsBlend {
    fn get_data(&self) -> Option<JiggleSettingsData> {
        let settings_count_space = self.settings.len()-1;
        let target_a : usize = (self.blend.floor() as usize).clamp(0, settings_count_space);
        let target_b : usize = ((self.blend.floor()+1.0) as usize).clamp(0, settings_count_space);
        let normalized_blend_clamp : f32 = (self.blend.clamp(0.0,1.0)*(settings_count_space as f32)-(target_a as f32)).clamp(0.0,1.0);
        let target_a : JiggleSettingsData = get_jiggle_parameters(self.settings.get(target_a))?;
        let target_b : JiggleSettingsData = get_jiggle_parameters(self.settings.get(target_b))?;
        Some(target_a.lerp(&target_b, normalized_blend_clamp))
    }
}

#[godot_api]
impl ResourceVirtual for JiggleSettingsBlend {
    fn init(_base: Base<Resource>) -> Self {
        Self {
            settings: godot::builtin::Array::new(),
            blend: 0.0,
        }
    }
}
