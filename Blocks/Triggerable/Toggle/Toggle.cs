using Godot;
using System;

public partial class Toggle : AbstractTriggerable {

	[ExportCategory("Attributes")]
	[Export] protected bool isRed = true;
	protected bool onCooldown = false;
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
		if(!timer.IsStopped())
			return;
		
        isRed = !isRed;
		ChangeState();
    }

	private void ChangeState() {
		if(isRed) {
			sprite.Texture = (Texture2D)GD.Load("res://Blocks/Triggerable/Toggle/RedToggle.png");
			GD.Print("Red");
		}
		else {
			sprite.Texture = (Texture2D)GD.Load("res://Blocks/Triggerable/Toggle/BlueToggle.png");
			GD.Print("Blue");
		}
		// emit signal and pass current state
		timer.Start();
	}
}
