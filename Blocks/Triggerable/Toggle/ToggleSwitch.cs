using Godot;
using System;

public partial class ToggleSwitch : AbstractTriggerable {

	[Signal]
	public delegate void ToggleEventHandler(bool isRed);

	[ExportCategory("Attributes")]
	[Export] protected static bool isRed = true;
	protected static Sprite2D sprite = new Sprite2D {
			Texture = (Texture2D)GD.Load("res://Blocks/Triggerable/Toggle/RedToggle.png")
		};
	protected static Timer timer = new Timer {
			WaitTime = 0.2,
			OneShot = true
		};
	protected static ToggleSwitch mainToggle;

	public override void _Ready() {
		if(!IsInstanceValid(timer.GetParent<Node2D>())) {
			mainToggle = this;
			AddChild(timer);
		}
		else {
			Toggle += mainToggle.RequestToggle;
		}
		mainToggle.Toggle += ChangeState;
	}

	private void RequestToggle(bool isRed) {
        EmitSignal(SignalName.Toggle, isRed);
    }

    public void SetIsRed(bool value) {
		isRed = value;
		ChangeState(isRed);
	}

	// changes the state of the toggle from red or blue
    public override void Entered(Node2D activator) {
		if(!timer.IsStopped())
			return;
		
        isRed = !isRed;
		EmitSignal(SignalName.Toggle, isRed);
    }

	private void ChangeState(bool isRed) {
		if(isRed) {
			sprite.Texture = (Texture2D)GD.Load("res://Blocks/Triggerable/Toggle/RedToggle.png");
		}
		else {
			sprite.Texture = (Texture2D)GD.Load("res://Blocks/Triggerable/Toggle/BlueToggle.png");
		}
		GetNode<Sprite2D>("Sprite2D").Texture = sprite.Texture;
		timer.Start();
	}
}
