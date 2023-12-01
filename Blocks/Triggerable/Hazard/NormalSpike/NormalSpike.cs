using Godot;
using System;

public partial class NormalSpike : AbstractHazard {
	// Entered scene tree
	public override void _Ready() {
                base._Ready();
                displayName = "Spike";
        }
}
