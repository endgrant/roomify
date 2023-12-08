using Godot;
using System;

public partial class PauseOverlay : CanvasLayer {
        private PackedScene playerScene = (PackedScene)GD.Load("res://Player/Player.tscn");
        private PackedScene levelSelectMenu = GD.Load<PackedScene>("res://Menus/LevelSelect/level_select.tscn");
        private PackedScene levelEditMenu = GD.Load<PackedScene>("res://Menus/LevelViewer/LevelEditor/level_editor.tscn");
        private Panel tester;
	private AudioStreamPlayer audio;
        private static Button editButton;
        private static Button selectButton;

	public override void _Ready() {
		base._Ready();
                tester = GetNode<Panel>("PlayTester");
		audio = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
                editButton = GetNode<Button>("Edit");
                selectButton = GetNode<Button>("Select");
                Visible = false;
	}


        public override void _Input(InputEvent @event) {
                base._Input(@event);
                if (@event.IsActionPressed("Pause")) {
                        if(Visible) {
                                Hide();
                        }
                        else {
                                Show();
                        }
                }
        }

        public void SliderChanged(float value) {
                audio.VolumeDb = value;
        }

        public new void Show() {
                Visible = true;
                Player player = (Player)playerScene.Instantiate();
                player.SetIsTester(true);
                AddChild(player);
                player.EnteredRoom(new Vector2(704, 544));
        }

        public new void Hide() {
                Visible = false;
                GetNode<Player>("Player").QueueFree();
        }

        public static void ChangedMenu(int menuType) {
                switch(menuType) {
                        case 0:
                                editButton.Visible = false;
                                selectButton.Visible = false;
                                break;
                        case 1:
                                editButton.Visible = true;
                                selectButton.Visible = true;
                                break;
                }
        }

        public void QuitToEditor() {
                Hide();
                GetTree().ChangeSceneToPacked(levelEditMenu);
        }

        public void QuitToSelector() {
                Hide();
                GetTree().ChangeSceneToPacked(levelSelectMenu);
        }
}
