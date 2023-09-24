use godot::prelude::*;
use godot::engine::Resource;
use godot::prelude::utilities::clampf;

#[derive(GodotClass)]
#[class(base=Resource)]
struct JiggleSettingsBase {
}

#[derive(GodotClass)]
#[class(base=JiggleSettingsBase)]
struct JiggleSettingsData {
    gravity: f32,
    friction: f32,
    air_friction: f32,
    blend: f32,
    angle_elasticity: f32,
    length_elasticity: f32,
    elasticity_soften: f32,
}

#[derive(GodotClass)]
#[class(base=JiggleSettingsBase)]
struct JiggleSettingsBlend {
    settings: Vec<JiggleSettingsBase>,
    blend: f32
}

trait JiggleSettings {
    fn get_parameters(&self) -> JiggleSettingsData;
}

impl JiggleSettings for JiggleSettingsBase {
    fn get_parameters(&self) {
        JiggleSettingsData {
            gravity: 0.0,
            friction: 0.0,
            air_friction: 0.0,
            blend: 0.0,
            angle_elasticity: 0.0,
            length_elasticity: 0.0,
            elasticity_soften: 0.0,
        }
    }
}

impl JiggleSettings for JiggleSettingsData {
    fn get_parameters(&self) -> JiggleSettingsData {
        &self
    }
}

impl JiggleSettings for JiggleSettingsBlend {
    fn get_parameters(&self) -> JiggleSettingsData {
        let settings_count_space = self.settings.len()-1;
        let target_a = self.blend.floor().clamp(0, settings_count_space);
        let target_b = (self.blend.floor()+1.0).clamp(0, settings_count_space);
        JiggleSettingsData {
            gravity: 0.0,
            friction: 0.0,
            air_friction: 0.0,
            blend: 0.0,
            angle_elasticity: 0.0,
            length_elasticity: 0.0,
            elasticity_soften: 0.0,
        }
    }
}
