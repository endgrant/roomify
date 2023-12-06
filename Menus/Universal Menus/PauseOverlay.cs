using Godot;
using System;

public partial class PauseOverlay : CanvasLayer {
        private PackedScene playerScene = (PackedScene)GD.Load("res://Player/Player.tscn");
        private Panel panel;
	private AudioStreamPlayer audio;

	public override void _Ready() {
		base._Ready();
                panel = GetNode<Panel>("PlayTester");
		audio = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
                Visible = false;
	}


        public override void _Input(InputEvent @event) {
                base._Input(@event);
                if (@event.IsActionPressed("Pause")) {
                        Visible = !Visible;
                        if(Visible) {
                                Player player = (Player)playerScene.Instantiate();
                                player.SetIsTester(true);
                                panel.AddChild(player);
                                player.Position = new Vector2(256, 352);
                        }
                        else {
                                panel.GetNode<Player>("Player").QueueFree();
                        }
                }
        }


        public void SliderChanged(float value) {
                audio.VolumeDb = value;
        }
}
