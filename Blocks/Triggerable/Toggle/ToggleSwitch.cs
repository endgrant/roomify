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
	protected static ToggleSwitch mainToggle = null;

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

	public bool GetIsRed() {
		return isRed;
	}

    public void SetIsRed(bool value) {
		isRed = value;
		ChangeState(isRed);
	}

	public static ToggleSwitch GetMainToggle() {
		return mainToggle;
	}

	public static void SetMainToggle(ToggleSwitch toggle) {
		mainToggle = toggle;
	}

	public void Disable() {
		Visible = false;
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = true;
	}

	public void Enable() {
		Visible = true;
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
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
