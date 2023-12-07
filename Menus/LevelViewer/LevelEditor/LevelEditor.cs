using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

public partial class LevelEditor : LevelViewer {
        [Signal]
        public delegate void NewEditEventHandler(AbstractBlock block);
        private PackedScene levelSelectMenu = GD.Load<PackedScene>("res://Menus/LevelSelect/level_select.tscn");
        private PackedScene defaultLevelScene = GD.Load<PackedScene>("res://Level/level.tscn");
        private PackedScene defaultRoomScene = GD.Load<PackedScene>("res://Blocks/Triggerable/Room/room.tscn");

        private Vector2I tileSize = new Vector2I(Constants.CELL_SIZE, Constants.CELL_SIZE);
        private Vector2 parentRoomPos;
        private bool hasGoal = false;
        private bool hasSpawn = false;
        private bool changingRooms = false;

        private Control topbar;
        private HBoxContainer editbar;
        private Label editBlockLabel;
        private Button deleteBlockButton;
        private Control spacer;
        private SubViewport viewport;
        private Level level;
        private TileMap ghostmap;
        private Node2D tiles;
        private TextureRect currentBlockTextureRect;
        private CanvasLayer prompt;

        private PackedScene currentBlock;
        private AbstractBlock currentEdit;
        private int atlasId;
        private int sourceId;
        private Vector2I currentCursorGridPos = Vector2I.Zero;
        private Vector2I prevCursorGridPos = Vector2I.Zero;


        // Entered scene
        public override void _Ready() {
                base._Ready();

                parentRoomPos = new Vector2(-96, -96);
                topbar = GetNode<Control>("VBoxContainer/Topbar");
                editbar = GetNode<HBoxContainer>("VBoxContainer/Editbar");
                editBlockLabel = editbar.GetNode<Label>("CurrentBlock");
                deleteBlockButton = editbar.GetNode<Button>("DeleteBlock");
                spacer = editbar.GetNode<Control>("Spacer");
                viewport = GetNode<SubViewport>("VBoxContainer/LevelViewport/SubViewport");
                ghostmap = viewport.GetNode<TileMap>("GhostMap");
                currentBlockTextureRect = GetNode<TextureRect>("VBoxContainer/Topbar/CurrentBlock/TextureRect");
                prompt = GetNode<CanvasLayer>("Prompt");

                Room startingRoom = defaultRoomScene.Instantiate<Room>();
                level = defaultLevelScene.Instantiate<Level>();
                level.masterRoom = startingRoom;
                level.currentRoom = startingRoom;
                tiles = level.GetNode<Node2D>("Tiles");
                level.currentRoom.SetTiles(tiles);

                if (!Constants.currentLevelName.Equals("")) {
                        // Load existing level
                        level.levelName = Constants.currentLevelName;
                        level.Load();
                } else {
                        level.levelName = "Unnamed_Level";
                }
                viewport.AddChild(level);

                SetCurrentBlock(0, GD.Load<PackedScene>("res://Blocks/Basic/BasicBlock/basic_block.tscn"), 1, 
                        topbar.GetNode<OptionButton>("BlockSelector/Block/Option").GetItemIcon(0));
                AbstractBlock.SetRoot(this);
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
                                level.currentRoom.DeleteBlock(currentCursorGridPos);
                        } else {
                                // Place block on left click
                               level.currentRoom.PlaceBlock(currentCursorGridPos, currentBlock); 
                        }
                }
                // Edit block on right click
                if (Input.IsActionJustPressed("Edit")) {
                        level.currentRoom.EditBlock(currentCursorGridPos);
                }
        }


        // Saves the level to file
        public void SaveLevel() {
                level.Save();
        }


        // Changes the current room
        public void ChangeCurrentRoom(Room newRoom) {
                changingRooms = true;
                foreach (AbstractBlock block in tiles.GetChildren()) {
                        if (IsInstanceValid(block)) {
                              level.currentRoom.DeleteBlock(block);  
                        }
                }
                SetEditedBlock(null);
                parentRoomPos = newRoom.GlobalPosition;
                level.currentRoom = newRoom;
                level.currentRoom.Load(level.currentRoom.GetRoomData());
                hasSpawn = false;
                foreach (AbstractBlock block in tiles.GetChildren()) {
                        if (IsInstanceValid(block) && block is Spawn) {
                              hasSpawn = true;  
                        }
                }
                changingRooms = false;
        }


        // Navigates up one room in the room tree
        public void NavPreviousRoom() {
                Room parentRoom = level.currentRoom.GetParentRoom();

                if (IsInstanceValid(parentRoom)) {
                        ChangeCurrentRoom(parentRoom);
                }
        }


        // Delete block button in edit bar clicked
        public void ManualDelete() {
                level.currentRoom.DeleteBlock(currentEdit);
                SetEditedBlock(null);
        }


        // Change currently selected block
        public void SetCurrentBlock(int atlasId, PackedScene blockScene, int sourceId, Texture2D texture) {
                this.atlasId = atlasId;
                this.sourceId = sourceId;
                currentBlock = blockScene;
                AbstractBlock instance = currentBlock.Instantiate<AbstractBlock>();
                currentBlockTextureRect.Texture = texture;
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
                EmitSignal("NewEdit", block);
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


        // Helper to create an integer selector on the edit bar
        public SpinBox CreateIntSelector(float value, string optionName, float min, float max, float step) {
                VBoxContainer container = new VBoxContainer();
                Label label = new Label();
                SpinBox selector = new SpinBox();

                container.CustomMinimumSize = new Vector2(192, 96);

                label.Text = optionName;
                label.HorizontalAlignment = HorizontalAlignment.Center;

                selector.Value = value;
                selector.MinValue = min;
                selector.MaxValue = max;
                selector.Step = step;

                container.AddChild(label);
                container.AddChild(selector);
                editbar.AddChild(container);

                return selector;
        }


        // Helper to create simple button
        public Button CreateButton(string buttonName) {
                Button button = new Button();

                button.CustomMinimumSize = new Vector2(192,96);
                button.Text = buttonName;                

                editbar.AddChild(button);

                return button;
        }

        public bool GetHasSpawn() {
                return hasSpawn;
        }

        public void SetHasSpawn(bool value) {
                hasSpawn = value;
        }

        public bool GetHasGoal() {
                return hasGoal;
        }

        public void SetHasGoal(bool value) {
                hasGoal = value;
        }


	// Quit button pressed
        public void Quit() {
                GetTree().ChangeSceneToPacked(levelSelectMenu);
        }

        public Vector2 GetParentRoomPos() {
                return parentRoomPos;
        }

        public bool IsChangingRooms() {
                return changingRooms;
        }

    public void OpenPrompt(string message) {
        prompt.GetNode<Button>("CenterContainer/Button").Text = message;
        prompt.Visible = true;
        ghostmap.Visible = false;
    }

    public void ExitPrompt() {
        prompt.Visible = false;
        ghostmap.Visible = true;
    }
}