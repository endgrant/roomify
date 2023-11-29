using Godot;
using System;

public partial class PeriodicSpike : AbstractHazard {
	protected Sprite2D sprite;
	protected Area2D hitbox;
	[ExportCategory("Attributes")]
	[Export(PropertyHint.Range, "0, 10")] protected float period;

	
}
