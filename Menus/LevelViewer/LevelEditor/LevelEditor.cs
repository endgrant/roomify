using Godot;
using System;

public partial class LevelEditor : LevelViewer
{
        private PackedScene levelSelectMenu = GD.Load<PackedScene>("res://Menus/LevelSelect/level_select.tscn");

        private Control topbar;
        private SubViewport viewport;
        private TileMap tilemap;
        private TextureRect currentBlockTextureRect;

        private AbstractBlock currentBlock;

        private PackedScene basicBlock = GD.Load<PackedScene>("res://Blocks/Basic/BasicBlock/basic_block.tscn");


        // Entered scene
        public override void _Ready() {
                base._Ready();
                topbar = GetNode<Control>("VBoxContainer/Topbar");
                viewport = GetNode<SubViewport>("VBoxContainer/LevelViewport/SubViewport");
                tilemap = viewport.GetNode<TileMap>("TileMap");
                currentBlockTextureRect = GetNode<TextureRect>("VBoxContainer/Topbar/CurrentBlock/TextureRect");

                SetCurrentBlock((AbstractBlock)basicBlock.Instantiate<Node2D>());
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
                tilemap.SetCell(0, cursorGridPos, 0, Vector2I.Zero, 1);
        }


        // Change currently selected block
        private void SetCurrentBlock(AbstractBlock block) {
                currentBlock = block;
                currentBlockTextureRect.Texture = currentBlock.GetTexture();
        }


	// Quit button pressed
        public void Quit() {
                GetTree().ChangeSceneToPacked(levelSelectMenu);
                
        }

}