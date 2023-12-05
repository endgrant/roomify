using Godot;
using System;

public partial class LevelSelect : Node
{
	private PackedScene mainMenu = GD.Load<PackedScene>("res://Menus/MainMenu/main_menu.tscn");
        private PackedScene levelEditorMenu = GD.Load<PackedScene>("res://Menus/LevelViewer/LevelEditor/level_editor.tscn");


	// Back button is pressed
        public void Back() {
                GetTree().ChangeSceneToPacked(mainMenu);
        }


        // Create button is pressed
        public void Create() {
                Constants.currentLevelName = "";
                GetTree().ChangeSceneToPacked(levelEditorMenu);
        }


        // A level is selected
        public void LevelSelected(string levelName) {
                Constants.currentLevelName = levelName;
        }


        // Edit selected level
        public void EditLevel() {
                GetTree().ChangeSceneToPacked(levelEditorMenu);
        }
}
