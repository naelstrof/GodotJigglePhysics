use godot::prelude::*;

#[derive(GodotClass)]
#[class(base=Node)]
struct JiggleRig {
    #[export]
    jiggle_settings : Option<Gd<Resource>>,
    //#[base]
    //base: Base<Node>,
}

#[godot_api]
impl JiggleRig {

}
