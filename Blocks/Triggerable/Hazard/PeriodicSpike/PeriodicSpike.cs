using Godot;
using System;

public partial class PeriodicSpike : AbstractHazard {
	[ExportCategory("Attributes")]
	[Export(PropertyHint.Range, "0.4, 6")] protected float period = 2.0F;
	[Export] protected bool active = true;
	protected Timer timer;
	protected AnimationPlayer animator;


        // Entered tree
        public override void _Ready() {
		base._Ready();
                displayName = "Periodic Spike";
		timer = GetNode<Timer>("Timer");
		animator = GetNode<AnimationPlayer>("AnimationPlayer");
		timer.WaitTime = period;
		timer.Start();
	}


        // Edit block
        public override void Edit() {
                base.Edit();
                HSlider slider = root.CreateSlider(period, "Period", 0.4F, 6.0F, 0.1F);
                Callable callable = new Callable(this, "PeriodChanged");
                slider.Connect("value_changed", callable);
        }


    private void PeriodChanged(float value) {
                period = value;
                timer.WaitTime = period;
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


        public override string Save() {
                return Json.Stringify(new Godot.Collections.Dictionary{
                        ["Path"] = "res://Blocks/Triggerable/Hazard/PeriodicSpike/periodic_spike.tscn",                      
                        ["Period"] = period
                });
        }


        public override void Load(Godot.Collections.Dictionary<string, Variant> data) {  
                base.Load(data);             
        }

}
