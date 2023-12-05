using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        
        private Room currentRoom;

        private PackedScene currentBlock;
        private AbstractBlock currentEdit;
        private int atlasId;
        private int sourceId;
        private Vector2I currentCursorGridPos = Vector2I.Zero;
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
                ghostmap = viewport.GetNode<TileMap>("GhostMap");
                tiles = level.GetNode<Node2D>("Tiles");
                currentBlockTextureRect = GetNode<TextureRect>("VBoxContainer/Topbar/CurrentBlock/TextureRect");

                currentRoom = new Room();
                currentRoom.SetTiles(tiles);

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

                currentCursorGridPos = cursorGridPos;
        }


        // Handles unhandled input
        public override void _UnhandledInput(InputEvent input) {
                if (input.IsActionPressed("Accept")) {
                        if (input.IsActionPressed("Delete")) {
                                // Delete block on ctrl left click
                                currentRoom.DeleteBlock(currentCursorGridPos);
                        } else {
                                // Place block on left click
                               currentRoom.PlaceBlock(currentCursorGridPos, currentBlock); 
                        }
                }
                // Edit block on right click
                if (Input.IsActionJustPressed("Edit")) {
                        currentRoom.EditBlock(currentCursorGridPos);
                }
        }


        // Saves the level to file
        public void SaveLevel() {
                int greatestFileName = 0;
                string[] fileNames = Directory.GetFiles(Constants.SAVE_DIR);
                foreach (string fileName in fileNames) {
                        try {
                                int fileIndex = Int32.Parse(fileName.TrimSuffix(".tscn").TrimPrefix(Constants.SAVE_DIR + "\\"));
                                if (fileIndex > greatestFileName) {
                                        greatestFileName = fileIndex;
                                }
                        } catch (Exception e) {
                                GD.Print(e);
                        };
                }
                greatestFileName++;

                foreach (Node child in tiles.GetChildren()) {
                        child.Owner = level;
                }

                PackedScene levelScene = new PackedScene();
                Error error = levelScene.Pack(level);
                if (error.Equals(Error.Ok)) {
                        ResourceSaver.Save(levelScene, Constants.SAVE_DIR + "/" + greatestFileName + ".tscn");
                }
        }
        

        // Changes the current room
        public void ChangeCurrentRoom(Room newRoom) {
                foreach (AbstractBlock block in tiles.GetChildren()) {
                        if (IsInstanceValid(block)) {
                              tiles.RemoveChild(block);  
                        }
                        
                }

                SetEditedBlock(null);
                currentRoom = newRoom;

                foreach (AbstractBlock block in currentRoom.GetCells()) {
                        if (IsInstanceValid(block)) {
                              tiles.AddChild(block);  
                        }  
                }
        }


        // Navigates up one room in the room tree
        public void NavPreviousRoom() {
                Room parentRoom = currentRoom.GetParentRoom();

                if (IsInstanceValid(parentRoom)) {
                        ChangeCurrentRoom(parentRoom);
                }
        }


        // Delete block button in edit bar clicked
        public void ManualDelete() {
                currentRoom.DeleteBlock(currentEdit);
                SetEditedBlock(null);
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
                ClearEditBar();
                string append = "None";
                if (IsInstanceValid(block)) {
                        append = block.GetDisplayName();
                        currentEdit = block;
                } else {
                        currentEdit = null;
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


        // Helper to create simple button
        public Button CreateButton(string buttonName) {
                Button button = new Button();

                button.CustomMinimumSize = new Vector2(192,96);
                button.Text = buttonName;                

                editbar.AddChild(button);

                return button;
        }


	// Quit button pressed
        public void Quit() {
                GetTree().ChangeSceneToPacked(levelSelectMenu);
        }

}
