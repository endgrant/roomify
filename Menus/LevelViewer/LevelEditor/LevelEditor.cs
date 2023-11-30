using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public partial class LevelEditor : LevelViewer
{
        private PackedScene levelSelectMenu = GD.Load<PackedScene>("res://Menus/LevelSelect/level_select.tscn");

        private Vector2I tileSize = new Vector2I(Constants.CELL_SIZE, Constants.CELL_SIZE);

        private Control topbar;
        private HBoxContainer editbar;
        private Label editBlockLabel;
        private Button deleteBlockButton;
        private Control spacer;
        private SubViewport viewport;
        private Node2D level;
        private TileMap ghostmap;
        private Node2D tiles;
        private TextureRect currentBlockTextureRect;
        
        private List<Room> rooms = new List<Room>();
        private Room currentRoom;

        private PackedScene currentBlock;
        private int atlasId;
        private int sourceId;
        private Vector2I prevCursorGridPos = Vector2I.Zero;


        // Entered scene
        public override void _Ready() {
                base._Ready();
                topbar = GetNode<Control>("VBoxContainer/Topbar");
                editbar = GetNode<HBoxContainer>("VBoxContainer/Editbar");
                editBlockLabel = editbar.GetNode<Label>("CurrentBlock");
                deleteBlockButton = editbar.GetNode<Button>("DeleteBlock");
                spacer = editbar.GetNode<Control>("Spacer");
                viewport = GetNode<SubViewport>("VBoxContainer/LevelViewport/SubViewport");
                level = viewport.GetNode<Node2D>("Level");
                ghostmap = level.GetNode<TileMap>("GhostMap");
                tiles = level.GetNode<Node2D>("Tiles");
                currentBlockTextureRect = GetNode<TextureRect>("VBoxContainer/Topbar/CurrentBlock/TextureRect");

                currentRoom = new Room(tiles);
                rooms.Add(currentRoom);

                SetCurrentBlock(0, GD.Load<PackedScene>("res://Blocks/Basic/BasicBlock/basic_block.tscn"), 1);
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
                if (cursorGridPos.X < 0 || cursorGridPos.Y < 0 || cursorGridPos.X > Constants.ROOM_WIDTH-1 || cursorGridPos.Y > Constants.ROOM_HEIGHT-1) {
                        return;
                }
                ghostmap.EraseCell(0, prevCursorGridPos);
                ghostmap.SetCell(0, cursorGridPos, atlasId, Vector2I.Right * (sourceId - 1), 0);
                prevCursorGridPos = cursorGridPos;

                // Place block on left click
                if (Input.IsActionPressed("Accept") && !Input.IsActionPressed("Delete")) {
                        currentRoom.PlaceBlock(cursorGridPos, currentBlock);
                }
                // Delete block on ctrl left click
                if (Input.IsActionPressed("Delete")) {
                        currentRoom.DeleteBlock(cursorGridPos);
                }
                // Edit block on right click
                if (Input.IsActionJustPressed("Edit")) {
                        currentRoom.EditBlock(cursorGridPos);
                }
        }


        // Change currently selected block
        public void SetCurrentBlock(int atlasId, PackedScene blockScene, int sourceId) {
                this.atlasId = atlasId;
                this.sourceId = sourceId;
                currentBlock = blockScene;
                AbstractBlock instance = currentBlock.Instantiate<AbstractBlock>();
                currentBlockTextureRect.Texture = instance.GetTexture();
                instance.QueueFree();
        }


        // Change currently edited block
        public void SetEditedBlock(AbstractBlock block) {
                string append = "None";
                if (IsInstanceValid(block)) {
                        append = block.Name;
                }
                editBlockLabel.Text = "Currently Editing: " + append;
        }


        // Clear edit bar
        public void ClearEditBar() {
                foreach (Control element in editbar.GetChildren()) {
                        if (element != editBlockLabel && element != deleteBlockButton && element != spacer) {
                                element.QueueFree();
                        }
                }
        }


        // Helper to create slider on edit bar
        public HSlider CreateSlider(float value, string optionName, float min, float max, float step) {
                VBoxContainer container = new VBoxContainer();
                Label label = new Label();
                HSlider slider = new HSlider();

                container.CustomMinimumSize = new Vector2(192, 96);

                label.Text = optionName;
                label.HorizontalAlignment = HorizontalAlignment.Center;

                slider.TicksOnBorders = true;
                slider.TickCount = 5;
                slider.MinValue = min;
                slider.MaxValue = max;
                slider.Step = step;
                slider.Value = value;

                container.AddChild(label);
                container.AddChild(slider);
                editbar.AddChild(container);

                return slider;
        }


	// Quit button pressed
        public void Quit() {
                GetTree().ChangeSceneToPacked(levelSelectMenu);
        }

}
