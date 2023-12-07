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
                if(!(((LevelPlayer)root).timer.WaitTime == 0))
                        return;

                if(activator is Player) {
                        ((LevelPlayer)root).timer.Start();
                        ((LevelPlayer)root).NavPreviousRoom();
                        ((Player)activator).EnterRoom(parentPos);
                }
        }


        public override string Save() {
                return Json.Stringify(new Godot.Collections.Dictionary{
                        ["Path"] = "res://Blocks/Triggerable/Spawn/spawn.tscn",    
                        ["X"] = parentPos.X,
                        ["Y"] = parentPos.Y              
                });
        }


        public override void Load(Godot.Collections.Dictionary<string, Variant> data) {  
                base.Load(data);             
        }
}
