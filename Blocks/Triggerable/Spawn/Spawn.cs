using Godot;
using System;

public partial class Spawn : AbstractTriggerable {
        private Vector2I parentPos;
        public static Player player;


	public override void _Ready() {
                base._Ready();
                displayName = "Spawn";
                parentPos = root.GetParentRoomPos();

                if (root is LevelPlayer) {
                        LevelPlayer levelPlayer = (LevelPlayer)root;
                        player = levelPlayer.GetPlayer();
                }

                if (IsInstanceValid(player)) {
                        player.EnteredRoom(Position);
                }
        }


        public override void Entered(Node2D activator) {
                if(!(((LevelPlayer)root).GetTimeLeft() == 0.0))
                        return;
                if(parentPos.X == -96 && parentPos.Y == -96)
                        return;

                if(activator is Player) {
                        root.NavPreviousRoom();
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
                parentPos = new Vector2I((int)data["EntranceX"], (int)data["EntranceY"]);       
        }
}
