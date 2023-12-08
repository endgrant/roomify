using Godot;
using System;
using System.Collections.Generic;
using System.IO.Enumeration;

public partial class Room : AbstractTriggerable
{
        private Node2D tiles;
	private Room parentRoom;
        public Vector2I parentPos = new Vector2I (-96, -96);
        private AbstractBlock[,] cells = new AbstractBlock[Constants.ROOM_WIDTH, Constants.ROOM_HEIGHT];
        private Godot.Collections.Dictionary<string, Variant> roomData;

        private Vector2I tileSize = new Vector2I(Constants.CELL_SIZE, Constants.CELL_SIZE);

        private Timer flush;


        public Room() {}


        // Entered scene tree
	public override void _Ready() {
                flush = GetNode<Timer>("Timer");

                base._Ready();
                displayName = "Room";
        }


        // Entered
        public override void Entered(Node2D activator) {
                if (!(activator is Player)) {
                        return;
                }
                activator.Position = new Vector2I(-96, -96);

                LevelPlayer levelPlayer = (LevelPlayer)root;
                levelPlayer.SetViewportVisibility(false);
                flush.Start();
        }


        public void Flushed() {
                LevelPlayer levelPlayer = (LevelPlayer)root;
                ChangeRoom();
                levelPlayer.SetViewportVisibility(true);
        }


        public override void Edit() {
                base.Edit();
                Button button = ((LevelEditor)root).CreateButton("Enter Room");
                Callable callable = new Callable(this, "ChangeRoom");
                button.Connect("pressed", callable);
        }


        // Changes the current room to this one
        public void ChangeRoom() {
                if (root is LevelEditor) {
                        if(!((LevelEditor)root).GetHasSpawn()) {
                                ((LevelEditor)root).OpenPrompt("Place a spawn before changing rooms!");
                                return;
                        }
                }
                root.ChangeCurrentRoom(this, false);
        }


        // Returns a list of all the blocks in this Room
        public AbstractBlock[,] GetCells() {
                return cells;
        }

        
        // Returns the tiles collection
        public Node2D GetTiles() {
                return tiles;
        }


        // Assigns the tiles collection
        public void SetTiles(Node2D tiles) {
                this.tiles = tiles;
        }


        // Assigns the parenting room
        public void SetParentRoom(Room room) {
                this.parentRoom = room;
        }


        // Returns the parenting room in the room tree
        public Room GetParentRoom() {
                return parentRoom;
        }


        // Assigns room data
        public void SetRoomData(Godot.Collections.Dictionary<string, Variant> data) {
                roomData = data;
        }


        // Returns this room's data
        public Godot.Collections.Dictionary<string, Variant> GetRoomData() {
                return roomData;
        }


        // Place block in room grid from scene
        public void PlaceBlock(Vector2I pos, PackedScene blockScene) {
                if(IsInstanceValid(GetBlockFromGrid(pos)))
                        return;
                AbstractBlock instance = blockScene.Instantiate<AbstractBlock>();
                if(instance is Spawn && ((LevelEditor)root).GetHasSpawn()) {
                        ((LevelEditor)root).OpenPrompt("There is already a spawn in this room!");
                        return;
                }
                if(instance is Goal && ((LevelEditor)root).GetHasGoal()) {
                        ((LevelEditor)root).OpenPrompt("There is already a goal in this level!");
                        return;
                }
                SetBlockInGrid(pos, instance);
                if(instance is Spawn)
                        ((LevelEditor)root).SetHasSpawn(true);
                if(instance is Goal)
                        ((LevelEditor)root).SetHasGoal(true);

        }


        // Place block in room grid from object
        public void PlaceBlock(Vector2I pos, AbstractBlock block) {
                if (!IsInstanceValid(GetBlockFromGrid(pos))) {
                        SetBlockInGrid(pos, block);
                }
        }


        // Remove block from room grid
        public void DeleteBlock(Vector2I pos) {
                AbstractBlock block = GetBlockFromGrid(pos);
                if (!IsInstanceValid(block))
                        return;
                if(block is Spawn)
                        ((LevelEditor)root).SetHasSpawn(false);
                if(block is Goal && !((LevelEditor)root).IsChangingRooms())
                        ((LevelEditor)root).SetHasGoal(false);

                tiles.RemoveChild(block);
                RemoveBlockFromGrid(pos);
        }


        // Remove block by reference
        public void DeleteBlock(AbstractBlock block) {
                if(!IsInstanceValid(block))
                        return;
                
                if (root is LevelEditor) {
                        if(block is Spawn)
                                ((LevelEditor)root).SetHasSpawn(false);
                        if(block is Goal && !((LevelEditor)root).IsChangingRooms())
                                ((LevelEditor)root).SetHasGoal(false);
                }
                
                tiles.RemoveChild(block);
                Vector2I gridPos = block.GetGridPosition();
                RemoveBlockFromGrid(gridPos);
        }


        // Begins editing the targeted block
        public void EditBlock(Vector2I pos) {
                AbstractBlock block = GetBlockFromGrid(pos);
                if (IsInstanceValid(block)) {
                        block.Edit();
                }
        }


        // Returns the block at the given grid position
        private AbstractBlock GetBlockFromGrid(Vector2I pos) {
                return cells[pos.X, pos.Y];
        }


        // Builds block in grid from object
        private void SetBlockInGrid(Vector2I pos, AbstractBlock block) {
                cells[pos.X, pos.Y] = block;
                block.Position = ((pos + Vector2I.One) * tileSize) - (tileSize / 2);

                if (block is Room) {
                        Room roomInstance = (Room)block;
                        roomInstance.SetTiles(tiles);
                        roomInstance.SetParentRoom(this);
                        roomInstance.parentPos = pos;
                }

                tiles.AddChild(block);
        }


        // Removes block from grid
        private void RemoveBlockFromGrid(Vector2I pos) {
                AbstractBlock instance = GetBlockFromGrid(pos);
                if (IsInstanceValid(instance)) {
                        instance.QueueFree();
                        cells[pos.X, pos.Y] = null;
                }
        }


        public override string Save() {
                Godot.Collections.Dictionary<string, Variant> dict = new();
                for (int i = 0; i < cells.GetLength(0); i++) {
                        for (int j = 0; j < cells.GetLength(1); j++) {
                                if (IsInstanceValid(cells[i, j])) {
                                        string data;
                                        if (cells[i, j] is Room) {
                                                Room subRoom = (Room)cells[i, j];
                                                data = Json.Stringify(subRoom.GetRoomData());
                                                if (data.Equals("null")) {
                                                        data = Json.Stringify(new Godot.Collections.Dictionary{
                                                                ["Path"] = "res://Blocks/Triggerable/Room/room.tscn",             
                                                                ["Cells"] = new Godot.Collections.Dictionary<string, Variant>() {}
                                                        });
                                                }
                                        } else {
                                                data = cells[i, j].Save();
                                        }
                                        dict.Add("[" + i.ToString() + "," + j.ToString() + "]", data);
                                }
                        }
                }

                return Json.Stringify(new Godot.Collections.Dictionary{
                        ["Path"] = "res://Blocks/Triggerable/Room/room.tscn",             
                        ["Cells"] = dict
                });
        }


        public override void Load(Godot.Collections.Dictionary<string, Variant> data) {
                if (data == null) {
                        return;
                }

                Godot.Collections.Dictionary<string, string> cells = (
                        Godot.Collections.Dictionary<string, string>)data["Cells"];

                foreach (System.Collections.Generic.KeyValuePair<string, string> keyValuePair in cells) {
                        Variant? internalData = Json.ParseString(keyValuePair.Value);
                        if (internalData == null) {
                                GD.Print("Parse Error");
                        }
                        Godot.Collections.Dictionary<string, Variant> internalDict = (Godot.Collections.Dictionary<string, Variant>)internalData;
                        GD.Print(internalDict);
                        PackedScene blockScene = GD.Load<PackedScene>((string)internalDict["Path"]);
                        AbstractBlock block = blockScene.Instantiate<AbstractBlock>();

                        // Load block internal data if its not a sub-Room
                        if (!(block is Room)) {
                                block.Load(internalDict);
                        } else {
                                // Store room data for later loading
                                Room subRoom = (Room)block;
                                subRoom.SetRoomData(internalDict);
                        }
                        
                        string[] stringVector = keyValuePair.Key.TrimPrefix("[").TrimSuffix("]").Split(",");
                        Vector2I gridPos = new Vector2I(Int32.Parse(stringVector[0]), Int32.Parse(stringVector[1]));
                        PlaceBlock(gridPos, block);
                }
        }
}
