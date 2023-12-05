using Godot;
using System;

public partial class Spawn : AbstractTriggerable {
    private Room linkedRoom = null;

	public override void _Ready() {
            base._Ready();
            displayName = "Spawn";
        }

    public override void Entered(Node2D activator) {
        
    }

}
