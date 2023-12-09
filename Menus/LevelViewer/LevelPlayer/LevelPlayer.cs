using Godot;
using System;

public partial class LevelPlayer : LevelViewer {
	private static PackedScene editor = GD.Load<PackedScene>("res://Menus/LevelViewer/LevelEditor/level_editor.tscn");
	private static PackedScene selector = GD.Load<PackedScene>("res://Menus/LevelSelect/level_select.tscn");
        private Timer timer;
        private Player player;
        private SubViewportContainer container;


        // Called when the node enters the scene tree for the first time.
        public override void _Ready() {
                container = GetNode<SubViewportContainer>("LevelViewport");
                viewport = container.GetNode<SubViewport>("SubViewport");
                player = viewport.GetNode<Player>("Player");
                Player.levelPlayer = this;

                base._Ready();

                timer = GetNode<Timer>("Timer");
                level.levelName = Constants.currentLevelName;
                level.Load();
                player.EnteredRoom(GetSpawnLocation(tiles));

                viewport.AddChild(level);
                previousMenu = this;
                EmitSignal(SignalName.MenuChanged, 1);
        }


        public void SetViewportVisibility(bool visible) {
                container.Visible = visible;
        } 


        public override void NavPreviousRoom() {
                if(!(GetTimeLeft() == 0))
                        return;
                base.NavPreviousRoom();
        }


        public override void ChangeCurrentRoom(Room newRoom, bool prev) {
                if(!(GetTimeLeft() == 0))
                        return;
                base.ChangeCurrentRoom(newRoom, prev);
                player.EnteredRoom(GetSpawnLocation(newRoom.GetTiles()));
        }


        private Vector2 GetSpawnLocation(Node2D tiles) {
                return tiles.GetNode<Spawn>("Spawn").GlobalPosition;
        }


        public void EndLevel() {
                ToggleHandler.instance.ResetToggle();
                if(previousMenu is LevelEditor) {
                        GetTree().ChangeSceneToPacked(editor);
                } else {
                        GetTree().ChangeSceneToPacked(selector);
                }
        }


        public double GetTimeLeft() {
                return timer.TimeLeft;
        }


        public void StartTimer() {
                timer.Start();
        }
}
