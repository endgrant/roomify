using Godot;
using System;

public partial class MainMenu : AbstractMenu
{
        private PackedScene levelSelectMenu = GD.Load<PackedScene>("res://Menus/LevelSelect/level_select.tscn");

    public override void _Ready() {
        base._Ready();
        HSlider volume = GetNode<HSlider>("AudioStreamPlayer/HSlider");
        volume.SetDeferred("value", 0);
        volume.EmitSignal("value_changed", 0);
    }

    // Levels button pressed
    public void Levels() {
                GetTree().ChangeSceneToPacked(levelSelectMenu);
        }


        // Quit button pressed
        public void Quit() {
                GetTree().Quit();
        }
}
