using Godot;
using System;
using System.IO;

public partial class LevelSelect : Node
{
	private PackedScene mainMenu = GD.Load<PackedScene>("res://Menus/MainMenu/main_menu.tscn");
        private PackedScene levelEditorMenu = GD.Load<PackedScene>("res://Menus/LevelViewer/LevelEditor/level_editor.tscn");

        private VBoxContainer main;
        private GridContainer grid;


        // Enter scene tree
        public override void _Ready() {
                base._Ready();
                main = GetNode<VBoxContainer>("VBoxContainer");
                grid = main.GetNode<GridContainer>("PageView/LevelView");

                string[] fileNames = Directory.GetFiles(Constants.SAVE_DIR);
                foreach (string fileName in fileNames) {
                        Button button = CreateButton(fileName.TrimSuffix(".lvl").TrimPrefix(Constants.SAVE_DIR + "\\"));
                        Action lambda = () => {LevelSelected(fileName);};
                        Callable callable = Callable.From(lambda);
                        button.Connect("pressed", callable);
                        grid.AddChild(button);
                }
        }


        // Creates level button
        private Button CreateButton(string levelName) {
                Button button = new Button();
                button.Text = levelName;
                button.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
                button.CustomMinimumSize = new Vector2(0, main.Size.Y / 3);
                return button;
        }


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


        // Delete selected level
        public void DeleteLevel() {
                string[] fileNames = Directory.GetFiles(Constants.SAVE_DIR);
                foreach (string fileName in fileNames) {
                        if (fileName.Equals(Constants.currentLevelName)) {
                                DirAccess.RemoveAbsolute(fileName);
                                break;
                        }
                }

                GetTree().ReloadCurrentScene();
        }
}
