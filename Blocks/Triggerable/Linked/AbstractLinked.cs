using Godot;
using System;

public abstract partial class AbstractLinked : AbstractTriggerable {
        public override void Entered(Node2D activator) {
                if(!(activator is Player))
                                return;
        }
}
