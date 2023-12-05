using Godot;
using System;

public partial class MainMenu : AbstractMenu
{
        private PackedScene levelSelectMenu = GD.Load<PackedScene>("res://Menus/LevelSelect/level_select.tscn");

    // Levels button pressed
    public void Levels() {
                GetTree().ChangeSceneToPacked(levelSelectMenu);
        }


        // Quit button pressed
        public void Quit() {
                GetTree().Quit();
        }
}
