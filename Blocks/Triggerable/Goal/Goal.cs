using Godot;
using System;

public partial class Goal : AbstractTriggerable
{       
        // Entered scene tree
	public override void _Ready() {
                base._Ready();
                displayName = "Goal";
        }


        public override void Entered(Node2D activator) {
                if(!(activator is Player))
			return;
                
                // TODO: Implement end of level
                GD.Print("Level Ended. Congratulations!");
        }


        public override string Save() {
                return Json.Stringify(new Godot.Collections.Dictionary{
                        ["Path"] = "res://Blocks/Triggerable/Goal/goal.tscn",
                        ["PosX"] = Position.X,
                        ["PosY"] = Position.Y,
                });
        }


        public override void Load(string data) {
                
        }
}
