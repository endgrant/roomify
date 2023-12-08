using Godot;
using System;

public partial class ToggleSwitch : AbstractTriggerable {
	[Signal]
	public delegate void ToggleEventHandler(bool isRed);
	protected static Sprite2D sprite = new Sprite2D {
					Texture = (Texture2D)GD.Load("res://Blocks/Triggerable/Toggle/RedToggle.png")
			};
			
	public ToggleSwitch() {
			
	}


	public override void _Ready() {
        displayName = "Switch";
		
		Toggle += ToggleHandler.instance.RequestToggle;
		ToggleHandler.instance.Toggle += ChangeState;
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
			sprite.Texture = (Texture2D)GD.Load("res://Blocks/Triggerable/Toggle/RedToggle.png");
		}
		else {
			sprite.Texture = (Texture2D)GD.Load("res://Blocks/Triggerable/Toggle/BlueToggle.png");
		}
		GetNode<Sprite2D>("Sprite2D").Texture = sprite.Texture;
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
