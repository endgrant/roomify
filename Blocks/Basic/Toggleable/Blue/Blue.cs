using Godot;
using System;

public partial class Blue : ToggleBlock {
        // Entered scene tree
	public override void _Ready() {
                base._Ready();
                displayName = "Blue Block";
        }


        protected override void ChangeState(bool isRed) {
                if(!isRed) {
                        sprite.Frame = 0;
                        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", false);
                }
                else {
                        sprite.Frame = 1;
                        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
                }
        }


        public override string Save() {
                return Json.Stringify(new Godot.Collections.Dictionary{
                        ["Path"] = "res://Blocks/Basic/Toggleable/Blue/blue_block.tscn"  
                });
        }


        public override void Load(Godot.Collections.Dictionary<string, Variant> data) {  
                base.Load(data);            
        }
}
