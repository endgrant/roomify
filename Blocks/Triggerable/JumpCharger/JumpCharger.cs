using Godot;
using System;

public partial class JumpCharger : AbstractTriggerable {

    [ExportCategory("Attributes")]
    [Export(PropertyHint.Range, "0, 10")] private int extraJumps = 3;

    private void GiveMoreJumps(Player player) {
        player.SetExtraJumps(extraJumps);
    }

    /*
    public override void Edit() {
        throw new NotImplementedException();
    }
    */

    public override void Entered(Node2D activator) {
        if(activator is Player)
            GiveMoreJumps((Player)activator);
    }
}
