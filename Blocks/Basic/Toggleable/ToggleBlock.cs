using Godot;
using System;

public abstract partial class ToggleBlock : AbstractBasic {
	// The toggle block scene inherits the basic block scene, meaning that
	// anything in the basic block scene will also automatically be applied
	// to the toggle block, such as sprite or hitbox
	protected Sprite2D sprite;
	protected CollisionShape2D hitbox;
	public override void _Ready() {
		sprite = GetNode<Sprite2D>("Sprite2D");
		hitbox = GetNode<CollisionShape2D>("CollisionShape2D");

                // ToggleBlock.cs
		ToggleHandler.instance.Connect(
                        ToggleHandler.SignalName.Toggle, new Callable(this, "ChangeState"));
		ChangeState(ToggleHandler.instance.GetIsRed());
	}


        // ToggleBlock.cs
        // Observes when the toggle switch signals activation
        protected abstract void ChangeState(bool isRed);
}
