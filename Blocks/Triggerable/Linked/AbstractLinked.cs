using Godot;
using System;

public partial class AbstractLinked : AbstractTriggerable {
    public override void Entered(Node2D activator) {
        if(!(activator is Player))
			return;
    }

}
