using Godot;
using System;
using System.IO;

public partial class LevelSelect : AbstractMenu {
	private PackedScene mainMenu = GD.Load<PackedScene>("res://Menus/MainMenu/main_menu.tscn");
        private PackedScene levelEditorMenu = GD.Load<PackedScene>("res://Menus/LevelViewer/LevelEditor/level_editor.tscn");
        private PackedScene levelSelectButton = GD.Load<PackedScene>("res://Menus/LevelSelect/level_select_button.tscn");
        private PackedScene levelPlayerMenu = GD.Load<PackedScene>("res://Menus/LevelViewer/LevelPlayer/level_player.tscn");

        private VBoxContainer main;
        private GridContainer grid;
        private VBoxContainer overlay;
        private LineEdit lineEdit;


        // Enter scene tree
        public override void _Ready() {
                base._Ready();
                main = GetNode<VBoxContainer>("VBoxContainer");
                grid = main.GetNode<GridContainer>("Center/PageView/LevelView");
                overlay = main.GetNode<VBoxContainer>("Center/RenameOverlay");
                lineEdit = overlay.GetNode<LineEdit>("LineEdit");
                previousMenu = this;

                if(!Directory.Exists(Constants.SAVE_DIR))
                        Directory.CreateDirectory(Constants.SAVE_DIR);
                string[] fileNames = DirAccess.GetFilesAt(Constants.SAVE_DIR);
                for (int i = (Constants.levelSelectPage-1) * 9; i < Constants.levelSelectPage * 9; i++) {
                        if (i + 1 > fileNames.Length) {
                                break;
                        }

                        string fileName = fileNames[i];
                        Button button = CreateButton(fileName.TrimSuffix(".lvl").TrimPrefix(Constants.SAVE_DIR + "\\"));
                        Action lambda = () => {LevelSelected(fileName);};
                        Callable callable = Callable.From(lambda);
                        button.Connect("pressed", callable);
                        grid.AddChild(button);
                }
        }


        // Creates level button
        private Button CreateButton(string levelName) {
                Button button = levelSelectButton.Instantiate<Button>();
                button.Text = levelName;
                button.CustomMinimumSize = new Vector2(0, grid.GetParentAreaSize().Y / 3);
                return button;
        }


	// Back button is pressed
        public void Back() {
                GetTree().ChangeSceneToPacked(mainMenu);
        }


        // Create button is pressed
        public void Create() {
                Constants.currentLevelName = "";
                lastButtonPressed = "create";
                GetTree().ChangeSceneToPacked(levelEditorMenu);
        }


        // A level is selected
        public void LevelSelected(string levelName) {
                Constants.currentLevelName = levelName;
        }


        // PLay select level
        public void PlayLevel() {
                if (Constants.currentLevelName.Equals("")) {
                        return;
                }
                lastButtonPressed = "play";
                GetTree().ChangeSceneToPacked(levelPlayerMenu);
        }


        // Edit selected level
        public void EditLevel() {
                if (Constants.currentLevelName.Equals("")) {
                        return;
                }
                lastButtonPressed = "edit";
                GetTree().ChangeSceneToPacked(levelEditorMenu);
        }


        // Delete selected level
        public void DeleteLevel() {
                if (Constants.currentLevelName.Equals("")) {
                        return;
                }
                string[] fileNames = DirAccess.GetFilesAt(Constants.SAVE_DIR);
                foreach (string fileName in fileNames) {
                        if (fileName.Equals(Constants.currentLevelName)) {
                                DirAccess.RemoveAbsolute(Constants.SAVE_DIR + "/" + fileName);
                                break;
                        }
                }
                lastButtonPressed = "delete";
                GetTree().ReloadCurrentScene();
        }


        // Change level name
        public void ChangeLevelName() { 
                if (Constants.currentLevelName.Equals("")) {
                        return;
                }               
                string[] fileNames = DirAccess.GetFilesAt(Constants.SAVE_DIR);
                foreach (string fileName in fileNames) {
                        if (fileName.Equals(Constants.currentLevelName)) {
                                // Remove a few forbidden characters
                                lineEdit.Text = lineEdit.Text.Replace(".", "");
                                lineEdit.Text = lineEdit.Text.Replace("\\", "");
                                lineEdit.Text = lineEdit.Text.Replace("/", "");

                                if (lineEdit.Text.Equals("")) {
                                        lineEdit.Text = "Unnamed_Level";
                                }

                                DirAccess.RenameAbsolute(Constants.SAVE_DIR + "/" + fileName, Constants.SAVE_DIR + "/" + lineEdit.Text + ".lvl");
                                break;
                        }
                }
                CancelNameChange();
                lastButtonPressed = "rename";
                GetTree().ReloadCurrentScene();
        }


        // Begin level name change
        public void BeginNameChange() {
                lineEdit.Text = "";
                overlay.Visible = true;
        }


        // Cancel level name change
        public void CancelNameChange() {
                overlay.Visible = false;
        }


        // Previous Constants.levelSelectPage
        public void PreviousPage() {
                Constants.levelSelectPage--;
                Constants.levelSelectPage = Math.Max(Constants.levelSelectPage, 1);

                GetTree().ReloadCurrentScene();
        }


        // Next Constants.levelSelectPage
        public void NextPage() {
                Constants.levelSelectPage++;
                string[] fileNames = DirAccess.GetFilesAt(Constants.SAVE_DIR);
                Constants.levelSelectPage = Math.Min(Constants.levelSelectPage, (int)Math.Ceiling(fileNames.Length / 9.0F));
                GetTree().ReloadCurrentScene();
        }
}
