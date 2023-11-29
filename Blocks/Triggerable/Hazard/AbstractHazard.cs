using Godot;
using System;

public partial class AbstractHazard : AbstractTriggerable {

    public override void Entered(Node2D activator) {
        if(activator is Player)
			Kill((Player)activator);
    }

    private void Kill(Player player) {
        player.Die();
    }

}
