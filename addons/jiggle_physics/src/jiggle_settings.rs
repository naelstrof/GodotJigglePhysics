use godot::prelude::*;

pub trait JiggleSettingsBaseVirtual {
    fn get_parameters(&self) -> JiggleSettingsData;
}

#[derive(GodotClass)]
#[class(init,base=Resource)]
struct JiggleSettingsBase { }


#[godot_api]
impl JiggleSettingsBase {}

impl JiggleSettingsBaseVirtual for JiggleSettingsBase {
    fn get_parameters(&self) -> JiggleSettingsData {
        JiggleSettingsData {
            gravity : 1.0,
            friction : 0.1,
            air_friction : 0.05,
            blend : 1.0,
            angle_elasticity : 0.4,
            length_elasticity : 0.8,
            elasticity_soften : 0.0,
        }
    }
}

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

impl JiggleSettingsBaseVirtual for JiggleSettingsData {
    fn get_parameters(&self) -> JiggleSettingsData {
        self.clone()
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

#[derive(GodotClass)]
#[class(base=Resource)]
struct JiggleSettingsBlend {
    #[export]
    settings: Vec<Option<Gd<dyn JiggleSettingsBaseVirtual>>>,
    #[init(default=0.0)]
    #[export(range=(0.0,1.0))]
    blend: f32
}

#[godot_api]
impl JiggleSettingsBlend {}

impl JiggleSettingsBaseVirtual for JiggleSettingsBlend {
    fn get_parameters(&self) -> JiggleSettingsData {
        let settings_count_space = self.settings.len()-1;
        let target_a : usize = (self.blend.floor() as usize).clamp(0, settings_count_space);
        let target_b : usize = ((self.blend.floor()+1.0) as usize).clamp(0, settings_count_space);
        let normalized_blend_clamp : f32 = (self.blend.clamp(0.0,1.0)*(settings_count_space as f32)-(target_a as f32)).clamp(0.0,1.0);
        let target_a : JiggleSettingsData = self.settings[target_a].get_parameters();
        let target_b : JiggleSettingsData = self.settings[target_b].get_parameters();
        target_a.lerp(&target_b, normalized_blend_clamp)
    }
}
