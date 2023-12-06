using Godot;
using System;

public partial class NormalSpike : AbstractHazard {
	// Entered scene tree
	public override void _Ready() {
                base._Ready();
                displayName = "Spike";
        }


        public override string Save() {
                return Json.Stringify(new Godot.Collections.Dictionary{
                        ["Path"] = "res://Blocks/Triggerable/Hazard/NormalSpike/normal_spike.tscn",
                        ["PosX"] = Position.X,
                        ["PosY"] = Position.Y,
                });
        }


        public override void Load(string data) {
                
        }
}
