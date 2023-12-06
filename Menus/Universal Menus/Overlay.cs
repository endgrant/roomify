using Godot;
using System;

public partial class Overlay : Control {
	private AudioStreamPlayer audio;

	public override void _Ready() {
		base._Ready();
		audio = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
	}


        public override void _Input(InputEvent @event) {
                base._Input(@event);
                if (@event.IsActionPressed("Pause")) {
                        Visible = !Visible;
                }
        }


        public void SliderChanged(float value) {
                audio.VolumeDb = value;
        }
}
