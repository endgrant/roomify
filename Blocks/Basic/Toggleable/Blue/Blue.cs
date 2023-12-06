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
                                sprite.FrameCoords = new Vector2I(0, 2);
                                GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", false);
                        }
                        else {
                                sprite.FrameCoords = new Vector2I(0, 4);
                                GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
                        }
        }


        public override string Save() {
                return Json.Stringify(new Godot.Collections.Dictionary{
                        ["Path"] = "res://Blocks/Basic/Toggleable/Blue/blue_block.tscn",
                        ["PosX"] = Position.X,
                        ["PosY"] = Position.Y,
                });
        }


        public override void Load(string data) {
                
        }
}
