using Godot;
using System;

public abstract partial class ToggleBlock : BasicBlock {
	// The toggle block scene inherits the basic block scene, meaning that
	// anything in the basic block scene will also automatically be applied
	// to the toggle block, such as sprite or hitbox
	protected Sprite2D sprite;
	protected CollisionShape2D hitbox;
	public override void _Ready() {
		sprite = GetNode<Sprite2D>("Sprite2D");
		hitbox = GetNode<CollisionShape2D>("CollisionShape2D");
		ToggleSwitch mainToggle;
		mainToggle = ToggleSwitch.GetMainToggle();
		if(!IsInstanceValid(mainToggle)) {
			mainToggle = new ToggleSwitch();
			AddSibling(mainToggle);
			ToggleSwitch.SetMainToggle(mainToggle);
			GD.Print("Added hidden toggle!");
		}
		mainToggle.Toggle += ChangeState;
		ChangeState(mainToggle.GetIsRed());
	}

    protected abstract void ChangeState(bool isRed);
}
