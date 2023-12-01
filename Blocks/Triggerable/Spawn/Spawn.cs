using Godot;
using System;

public partial class Spawn : AbstractTriggerable
{
	// Entered scene tree
	public override void _Ready() {
                base._Ready();
                displayName = "Spawn";
        }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void Entered(Node2D activator)
    {
        throw new NotImplementedException();
    }

}
