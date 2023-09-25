use godot::prelude::*;
use godot::engine::Skeleton3D;
use godot::engine::Skeleton3DVirtual;

#[derive(GodotClass)]
#[class(base=Skeleton3D)]
struct JiggleSkeleton {
    #[base]
    base: Base<Skeleton3D>
}

#[godot_api]
impl Skeleton3DVirtual for JiggleSkeleton {
    fn init(base: Base<Skeleton3D>) -> Self {
        Self {
            base,
        }
    }
}
