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
                if(!(((LevelPlayer)root).GetTimeLeft() == 0.0))
                        return;
                if(parentPos.X == -96 && parentPos.Y == -96)
                        return;

                if(activator is Player) {
                        ((Player)activator).EnteredRoom(parentPos);
                        ((LevelPlayer)root).NavPreviousRoom();
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
