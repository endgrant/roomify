using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class LevelEditor : LevelViewer
{
        private PackedScene levelSelectMenu = GD.Load<PackedScene>("res://Menus/LevelSelect/level_select.tscn");

        private Control topbar;
        private SubViewport viewport;
        private TileMap tilemap;
        private TextureRect currentBlockTextureRect;
        
        private AbstractBlock currentBlock;
        private Vector2I prevCursorGridPos = Vector2I.Zero;


        // Entered scene
        public override void _Ready() {
                base._Ready();
                topbar = GetNode<Control>("VBoxContainer/Topbar");
                viewport = GetNode<SubViewport>("VBoxContainer/LevelViewport/SubViewport");
                tilemap = viewport.GetNode<TileMap>("TileMap");
                currentBlockTextureRect = GetNode<TextureRect>("VBoxContainer/Topbar/CurrentBlock/TextureRect");

                SetCurrentBlock( GD.Load<PackedScene>("res://Blocks/Basic/BasicBlock/basic_block.tscn"));
        }


        // Runs every frame
        public override void _Process(double delta) {
                base._Process(delta);

                // Snap block preview to placement grid
                Vector2 viewportSize = viewport.Size;
                Vector2 windowSize = 
                        new Vector2((float)ProjectSettings.GetSetting("display/window/size/viewport_width"),
                                (float)ProjectSettings.GetSetting("display/window/size/viewport_height"));
                Vector2 scale = windowSize / viewportSize;
                Vector2I cursorGridPos = tilemap.LocalToMap(GetLocalMousePosition() * scale - new Vector2(0, topbar.Size.Y * scale.Y));
                tilemap.SetCell(1, prevCursorGridPos, -1, new Vector2I(-1, -1), -1);
                tilemap.SetCell(1, cursorGridPos, 1, Vector2I.Zero, 0);
                prevCursorGridPos = cursorGridPos;

                // Place block on left click
                if (Input.IsActionPressed("Accept")) {
                        tilemap.SetCell(0, cursorGridPos, 1, Vector2I.Zero, 0);
                }
                if (Input.IsActionPressed("Delete")) {
                        tilemap.SetCell(0, cursorGridPos, -1, new Vector2I(-1, -1), -1);
                }
        }


        // Place block
        private void PlaceBlock(Vector2I pos) {
                
        }


        // Change currently selected block
        public void SetCurrentBlock(PackedScene blockScene) {
                currentBlock = blockScene.Instantiate<AbstractBlock>();
                currentBlockTextureRect.Texture = currentBlock.GetTexture();
        }


	// Quit button pressed
        public void Quit() {
                GetTree().ChangeSceneToPacked(levelSelectMenu);
                
        }

}
