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
        private int sourceId;
        private Vector2I prevCursorGridPos = Vector2I.Zero;


        // Entered scene
        public override void _Ready() {
                base._Ready();
                topbar = GetNode<Control>("VBoxContainer/Topbar");
                viewport = GetNode<SubViewport>("VBoxContainer/LevelViewport/SubViewport");
                tilemap = viewport.GetNode<TileMap>("TileMap");
                currentBlockTextureRect = GetNode<TextureRect>("VBoxContainer/Topbar/CurrentBlock/TextureRect");

                SetCurrentBlock(GD.Load<PackedScene>("res://Blocks/Basic/BasicBlock/basic_block.tscn"), 1);
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
                DeleteBlock(prevCursorGridPos, 1);
                PlaceBlock(cursorGridPos, 1);
                prevCursorGridPos = cursorGridPos;

                // Place block on left click
                if (Input.IsActionPressed("Accept") && !Input.IsActionPressed("Delete")) {
                        PlaceBlock(cursorGridPos, 0);
                }
                if (Input.IsActionPressed("Delete")) {
                        DeleteBlock(cursorGridPos, 0);
                }
        }


        // Place block
        private void PlaceBlock(Vector2I pos, int layer) {
                tilemap.SetCell(layer, pos, layer, Vector2I.Right * (sourceId - 1) * layer, sourceId * Math.Abs(layer - 1));
        }


        // Delete block
        private void DeleteBlock(Vector2I pos, int layer) {
                tilemap.EraseCell(layer, pos);
                if(layer == 1)
                        return;
                        
                foreach(AbstractBlock element in tilemap.GetChildren()) {
                        element.QueueFree();
                }
        }


        // Change currently selected block
        public void SetCurrentBlock(PackedScene blockScene, int sourceId) {
                this.sourceId = sourceId;
                currentBlock = blockScene.Instantiate<AbstractBlock>();
                currentBlockTextureRect.Texture = currentBlock.GetTexture();
        }


	// Quit button pressed
        public void Quit() {
                GetTree().ChangeSceneToPacked(levelSelectMenu);
        }

}
