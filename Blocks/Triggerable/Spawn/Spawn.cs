using Godot;
using System;

public partial class Spawn : AbstractTriggerable {
        private Room linkedRoom = null;
        private Vector2 parentPos;

	public override void _Ready() {
                base._Ready();
                displayName = "Spawn";
                parentPos = ((LevelEditor)root).GetParentRoomPos();
        }


        public override void Entered(Node2D activator) {
                if(!(((LevelPlayer)root).timer.WaitTime == 0))
                        return;
                if(parentPos.X == -1 && parentPos.Y == 1)
                        return;

                if(activator is Player) {
                        ((LevelPlayer)root).timer.Start();
                        ((LevelPlayer)root).NavPreviousRoom();
                        ((Player)activator).EnteredRoom(parentPos);
                }
        }


        public override string Save() {
                return Json.Stringify(new Godot.Collections.Dictionary{
                        ["Path"] = "res://Blocks/Triggerable/Spawn/spawn.tscn",    
                        ["EntranceX"] = parentPos.X,
                        ["EntranceY"] = parentPos.Y              
                });
        }


        public override void Load(Godot.Collections.Dictionary<string, Variant> data) {  
                base.Load(data);
                parentPos = new Vector2((float)data["EntranceX"], (float)data["EntranceY"]);       
        }
}
