using Godot;
using System;

public partial class AudioModifier : AudioStreamPlayer {
	private HSlider slider;

	public override void _Ready() {
		base._Ready();
		slider = GetNode<HSlider>("HSlider");
		slider.SetDeferred("min_value", -80);
		slider.SetDeferred("max_value", 24);
	}

	public void SliderChanged(float value) {
		SetDeferred("volume_db", value);
	}
}
