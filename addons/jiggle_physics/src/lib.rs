mod jiggle_settings;
mod jiggle_skeleton;
//mod jiggle_rig;

use godot::prelude::*;

struct MyExtension;

#[gdextension]
unsafe impl ExtensionLibrary for MyExtension {}
