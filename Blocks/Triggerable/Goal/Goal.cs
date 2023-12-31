using Godot;
using System;

public partial class Goal : AbstractTriggerable
{       
        private Timer timer;


        // Entered scene tree
	public override void _Ready() {
                timer = GetNode<Timer>("Timer");

                base._Ready();
                displayName = "Goal";
        }


        public override void Entered(Node2D activator) {
                LevelPlayer levelPlayer = (LevelPlayer)root;

                if (timer.TimeLeft > 0.05) {
                        return;
                }

                if(!(activator is Player))
			return;
                ((Player)activator).Win();
        }


        public override string Save() {
                return Json.Stringify(new Godot.Collections.Dictionary{
                        ["Path"] = "res://Blocks/Triggerable/Goal/goal.tscn"      
                });
        }


        public override void Load(Godot.Collections.Dictionary<string, Variant> data) {  
                base.Load(data);            
        }
}
