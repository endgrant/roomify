using Godot;
using System;

public partial class Teleporter : AbstractLinked {
        private Vector2 target;

        
	// Entered scene tree
	public override void _Ready() {
                base._Ready();
                displayName = "Teleporter";
        }


        public void Connect(Teleporter targetTeleporter) {
                target = targetTeleporter.Position;
        }


        public override void Entered(Node2D activator) {
                base.Entered(activator);
                Warp((Player)activator);
        }


        private void Warp(Player player) {
                player.Position = target;
        }


        public override string Save() {
                return Json.Stringify(new Godot.Collections.Dictionary{
                        ["Path"] = "res://Blocks/Triggerable/Linked/Teleporter/teleporter.tscn",
                        ["PosX"] = Position.X,
                        ["PosY"] = Position.Y,
                        ["TargetX"] = target.X,
                        ["TargetY"] = target.Y
                });
        }


        public override void Load(string data) {
                
        }
}
