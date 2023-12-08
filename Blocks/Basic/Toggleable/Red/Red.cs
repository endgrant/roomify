using Godot;
using System;

public partial class Red : ToggleBlock {
        // Entered scene tree
	public override void _Ready() {
                base._Ready();
                displayName = "Red Block";
        }


        protected override void ChangeState(bool isRed) {
                if(isRed) {
                        sprite.FrameCoords = new Vector2I(0, 1);
                        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", false);
                }
                else {
                        sprite.FrameCoords = new Vector2I(0, 3);
                        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
                }
        }


        public override string Save() {
               return Json.Stringify(new Godot.Collections.Dictionary{
                        ["Path"] = "res://Blocks/Basic/Toggleable/Red/red_block.tscn"        
                });
        }


        public override void Load(Godot.Collections.Dictionary<string, Variant> data) {  
                base.Load(data);              
        }
}
