using Godot;
using System;

public abstract partial class AbstractTriggerable : AbstractBlock
{
	public abstract void Entered(Node2D activator);
}
