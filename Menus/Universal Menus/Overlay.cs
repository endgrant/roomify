using Godot;
using System;

public partial class Overlay : Node2D {
	private AudioStreamPlayer audio;

	public override void _Ready() {
		base._Ready();
		audio = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
	}

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
		if(Input.IsActionJustReleased("Pause"))
			SetDeferred("visible", !IsVisibleInTree());
    }

    public void SliderChanged(float value) {
		audio.VolumeDb = value;
	}
}
