using Godot;
using System;

public partial class JumpCharger : AbstractTriggerable {

    [ExportCategory("Attributes")]
    [Export(PropertyHint.Range, "0, 10")] protected int extraJumps = 3;


        // Entered scene tree
	public override void _Ready() {
                base._Ready();
                displayName = "Jump Charger";
        }


        // Grant more jumps to the player
        private void GiveMoreJumps(Player player) {
                player.SetExtraJumps(extraJumps);
        }


        public override void Edit() {

        }


        public override void Entered(Node2D activator) {
                if(activator is Player)
                GiveMoreJumps((Player)activator);
        }
}
