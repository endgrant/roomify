using Godot;
using System;

public partial class Teleporter : AbstractLinked
{
	// Entered scene tree
	public override void _Ready() {
                base._Ready();
                displayName = "Teleporter";
        }
}
