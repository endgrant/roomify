using Godot;
using System;

public partial class Spawn : AbstractTriggerable {
        private Room linkedRoom = null;


	public override void _Ready() {
            base._Ready();
            displayName = "Spawn";
        }


        public override void Entered(Node2D activator) {
                
        }


        public override string Save() {
                return Json.Stringify(new Godot.Collections.Dictionary{
                        ["Path"] = "res://Blocks/Triggerable/Spawn/spawn.tscn",
                        ["PosX"] = Position.X,
                        ["PosY"] = Position.Y,
                });
        }


        public override void Load(Godot.Collections.Dictionary<string, Variant> data) {  
                base.Load(data);             
        }
}
