using Godot;
using System;

public partial class Teleporter : AbstractLinked {
        private Teleporter target;
	// Entered scene tree
	public override void _Ready() {
                base._Ready();
                displayName = "Teleporter";
        }

        public void Connect(Teleporter target) {
                this.target = target;
        }

        public override void Entered(Node2D activator) {
                base.Entered(activator);
                target.Warp((Player)activator);
        }

        public void Warp(Player player) {
                player.GlobalPosition = GlobalPosition;
        }
}
