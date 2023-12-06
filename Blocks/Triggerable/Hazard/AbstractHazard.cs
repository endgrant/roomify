using Godot;
using System;

public abstract partial class AbstractHazard : AbstractTriggerable {
        // if the entity that entered it is the player, this object kills the player
        public override void Entered(Node2D activator) {
                if(activator is Player)
                                ((Player)activator).Die();
        }
}
