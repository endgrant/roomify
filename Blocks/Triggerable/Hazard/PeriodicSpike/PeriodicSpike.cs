using Godot;
using System;

public partial class PeriodicSpike : AbstractHazard {
	protected Sprite2D sprite;
	protected CollisionShape2D hitbox;
	protected Timer timer;
	protected AnimationPlayer animator;
	[ExportCategory("Attributes")]
	[Export(PropertyHint.Range, "0.2, 10")] protected float period = 2;
	[Export] protected bool active = true;

	public override void _Ready() {
		base._Ready();
		sprite = GetNode<Sprite2D>("Sprite2D");
		hitbox = GetNode<CollisionShape2D>("CollisionShape2D");
		timer = GetNode<Timer>("Timer");
		animator = GetNode<AnimationPlayer>("AnimationPlayer");
		timer.WaitTime = period;
		// TODO: If spike starts as not active, close the spike
		timer.Start();
	}

	public void Cycle() {
		timer.Start();
		if(active)
			animator.Play("Close");
		else
			animator.Play("Open");
		active = !active;
	}
}
