using Godot;
using System;

public partial class PeriodicSpike : AbstractHazard {
	[ExportCategory("Attributes")]
	[Export(PropertyHint.Range, "0.2, 10")] protected float period = 2;
	[Export] protected bool active = true;
	protected Timer timer;
	protected AnimationPlayer animator;


        // Entered tree
        public override void _Ready() {
		base._Ready();
		timer = GetNode<Timer>("Timer");
		animator = GetNode<AnimationPlayer>("AnimationPlayer");
		timer.WaitTime = period;
		timer.Start();
	}


        // Edit block
        public override void Edit()
        {
                base.Edit();
        }


        public void SetActive(bool value) {
		active = value;
		ChangeState();
	}


	public void Cycle() {
		timer.Start();
		active = !active;
		ChangeState();
	}


	private void ChangeState() {
		if(active)
			animator.Play("Open");
		else
			animator.Play("Close");
	}
}
