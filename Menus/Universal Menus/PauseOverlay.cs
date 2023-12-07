using Godot;
using System;

public partial class PauseOverlay : CanvasLayer {
        private PackedScene playerScene = (PackedScene)GD.Load("res://Player/Player.tscn");
        private Panel tester;
	private AudioStreamPlayer audio;

	public override void _Ready() {
		base._Ready();
                tester = GetNode<Panel>("PlayTester");
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
                                AddChild(player);
                                player.EnteredRoom(new Vector2(704, 544));
                        }
                        else {
                                GetNode<Player>("Player").QueueFree();
                        }
                }
        }

        public void SliderChanged(float value) {
                audio.VolumeDb = value;
        }
}
