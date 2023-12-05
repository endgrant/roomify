using Godot;
using System;

public partial class AudioModifier : AudioStreamPlayer {
	private HSlider slider;
	private static float volume = 0;

	public override void _Ready() {
		base._Ready();
		slider = GetNode<HSlider>("HSlider");
		slider.SetDeferred("min_value", -80);
		slider.SetDeferred("max_value", 24);
	}

	public void SliderChanged(float value) {
		volume = value;
		SetDeferred("volume_db", value);
	}
}
