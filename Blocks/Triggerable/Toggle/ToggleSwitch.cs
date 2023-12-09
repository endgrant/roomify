using Godot;
using System;

public partial class ToggleSwitch : AbstractTriggerable {
	[Signal]
	public delegate void ToggleEventHandler(bool isRed);
	private Sprite2D sprite;
			
	public ToggleSwitch() {
			
	}


	public override void _Ready() {
        displayName = "Switch";
		sprite = GetNode<Sprite2D>("Sprite2D");

		ToggleHandler.instance.Connect(SignalName.Toggle, new Callable(this, "ChangeState"));
		bool isRed = ToggleHandler.instance.GetIsRed();
		if(!isRed)
			ChangeState(isRed);
	}


	// changes the state of the toggle from red or blue
        public override void Entered(Node2D activator) {
			if(!ToggleHandler.instance.IsTimerStopped())
				return;
			if(!(activator is Player))
				return;
			ToggleHandler.instance.SetIsRed(!ToggleHandler.instance.GetIsRed());
        }


	protected void ChangeState(bool isRed) {
		if(isRed) {
			sprite.Frame = 0;
		}
		else {
			sprite.Frame = 1;
		}
		ToggleHandler.instance.StartTimer();
	}


        public override string Save() {
                return Json.Stringify(new Godot.Collections.Dictionary{
                        ["Path"] = "res://Blocks/Triggerable/Toggle/toggle_switch.tscn",                 
                });
        }


        public override void Load(Godot.Collections.Dictionary<string, Variant> data) {  
            base.Load(data);
        }
}
