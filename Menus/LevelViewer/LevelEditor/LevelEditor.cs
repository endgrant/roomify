using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class LevelEditor : LevelViewer
{
        private PackedScene levelSelectMenu = GD.Load<PackedScene>("res://Menus/LevelSelect/level_select.tscn");

        private const int tileWidth = 64;
        private const int tileHeight = 64;
        private const int roomWidth = 24;
        private const int roomHeight = 14;

        private Vector2I tileSize = new Vector2I(tileWidth, tileHeight);

        private Control topbar;
        private SubViewport viewport;
        private TileMap ghostmap;
        private TextureRect currentBlockTextureRect;
        
        private AbstractBlock[,] cells = new AbstractBlock[24, 14]; // prob shouldn't hard code

        private PackedScene currentBlock;
        private int sourceId;
        private Vector2I prevCursorGridPos = Vector2I.Zero;


        // Entered scene
        public override void _Ready() {
                base._Ready();
                topbar = GetNode<Control>("VBoxContainer/Topbar");
                viewport = GetNode<SubViewport>("VBoxContainer/LevelViewport/SubViewport");
                ghostmap = viewport.GetNode<TileMap>("GhostMap");
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
                Vector2I cursorGridPos = (Vector2I)(GetLocalMousePosition() * scale - new Vector2(0, topbar.Size.Y * scale.Y)) / tileSize;

                // Check if cursor is in-bounds
                if (cursorGridPos.X < 0 || cursorGridPos.Y < 0 || cursorGridPos.X > roomWidth-1 || cursorGridPos.Y > roomHeight-1) {
                        return;
                }
                ghostmap.EraseCell(0, prevCursorGridPos);
                ghostmap.SetCell(0, cursorGridPos, 0, Vector2I.Right * (sourceId - 1), 0);
                prevCursorGridPos = cursorGridPos;

                // Place block on left click
                if (Input.IsActionPressed("Accept") && !Input.IsActionPressed("Delete")) {
                        PlaceBlock(cursorGridPos);
                }
                // Delete block on ctrl left click
                if (Input.IsActionPressed("Delete")) {
                        DeleteBlock(cursorGridPos);
                }
        }


        // Place block
        private void PlaceBlock(Vector2I pos) {
                if (!IsInstanceValid(GetBlockFromGrid(pos))) {
                        SetBlockInGrid(pos);
                }
        }


        // Delete block
        private void DeleteBlock(Vector2I pos) {
                if (IsInstanceValid(GetBlockFromGrid(pos))) {
                        RemoveBlockFromGrid(pos);
                }
        }

        
        // Returns the block at the given grid position
        private AbstractBlock GetBlockFromGrid(Vector2I pos) {
                return cells[pos.X, pos.Y];
        }


        // Builds block in grid
        private void SetBlockInGrid(Vector2I pos) {
                AbstractBlock instance = currentBlock.Instantiate<AbstractBlock>();
                cells[pos.X, pos.Y] = instance;
                instance.Position = ((pos + Vector2I.One) * tileSize) - (tileSize / 2);
                viewport.AddChild(instance);
        }


        // Removes block from grid
        private void RemoveBlockFromGrid(Vector2I pos) {
                AbstractBlock instance = GetBlockFromGrid(pos);
                if (IsInstanceValid(instance)) {
                        instance.QueueFree();
                        cells[pos.X, pos.Y] = null;
                }
        }


        // Change currently selected block
        public void SetCurrentBlock(PackedScene blockScene, int sourceId) {
                this.sourceId = sourceId;
                currentBlock = blockScene;
                AbstractBlock instance = currentBlock.Instantiate<AbstractBlock>();
                currentBlockTextureRect.Texture = instance.GetTexture();
                instance.QueueFree();
        }


	// Quit button pressed
        public void Quit() {
                GetTree().ChangeSceneToPacked(levelSelectMenu);
        }

}
