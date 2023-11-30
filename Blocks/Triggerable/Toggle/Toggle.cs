using Godot;
using System;

public partial class Toggle : AbstractTriggerable {

	[ExportCategory("Attributes")]
	[Export] protected bool isRed = true;
	Sprite2D sprite;
	Timer timer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		sprite = GetNode<Sprite2D>("Sprite2D");
		timer = GetNode<Timer>("Timer");
	}

	public void SetIsRed(bool value) {
		isRed = value;
		ChangeState();
	}

	// changes the state of the toggle from red or blue
    public override void Entered(Node2D activator) {
        isRed = !isRed;
		ChangeState();
    }

	private void ChangeState() {
		if(!(timer.WaitTime == 0))
			return;

		if(isRed) {
			sprite.Texture = (Texture2D)GD.Load("res://Blocks/Triggerable/Toggle/RedToggle.png");
		}
		else {
			sprite.Texture = (Texture2D)GD.Load("res://Blocks/Triggerable/Toggle/BlueToggle.png");
		}
		// emit signal and pass current state
		timer.Start();
	}
}
