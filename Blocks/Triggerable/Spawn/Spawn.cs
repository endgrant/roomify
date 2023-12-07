using Godot;
using System;

public partial class Spawn : AbstractTriggerable {
        private Room linkedRoom = null;
        private Vector2 parentPos;

	public override void _Ready() {
                base._Ready();
                displayName = "Spawn";
        }


        public override void Entered(Node2D activator) {
                if(activator is Player)
                        ((LevelPlayer)root).NavPreviousRoom();
        }


        public override string Save() {
                return Json.Stringify(new Godot.Collections.Dictionary{
                        ["Path"] = "res://Blocks/Triggerable/Spawn/spawn.tscn",                  
                });
        }


        public override void Load(Godot.Collections.Dictionary<string, Variant> data) {  
                base.Load(data);             
        }
}
