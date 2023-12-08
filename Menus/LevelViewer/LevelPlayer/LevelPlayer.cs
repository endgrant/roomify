using Godot;
using System;

public partial class LevelPlayer : LevelViewer {
	private static PackedScene editor = GD.Load<PackedScene>("res://Menus/LevelViewer/LevelEditor/level_editor.tscn");
	private static PackedScene selector = GD.Load<PackedScene>("res://Menus/LevelSelect/level_select.tscn");

        private static bool isEditing;
        private Timer timer;


        // Called when the node enters the scene tree for the first time.
        public override void _Ready() {
                timer = GetNode<Timer>("Timer");
        }


        public void NavPreviousRoom() {
                if(!(timer.WaitTime == 0))
                        return;
                //navigate to previous room
        }


        public void EndLevel() {
                if(isEditing) {
                        GetTree().ChangeSceneToPacked(editor);
                } else {
                        GetTree().ChangeSceneToPacked(selector);
                }
        }


        public void SetIsEditing(bool value) {
                isEditing = value;
        }


        public double GetTimeLeft() {
                return timer.WaitTime;
        }


        public void StartTimer() {
                timer.Start();
        }
}
