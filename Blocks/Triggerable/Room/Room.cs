using Godot;
using System;
using System.Collections.Generic;
using System.IO.Enumeration;

public partial class Room : AbstractTriggerable
{
        private Node2D tiles;
	private Room parentRoom;
        private AbstractBlock[,] cells = new AbstractBlock[Constants.ROOM_WIDTH, Constants.ROOM_HEIGHT];
        private List<Room> rooms = new List<Room>();

        private Vector2I tileSize = new Vector2I(Constants.CELL_SIZE, Constants.CELL_SIZE);


        public Room() {}


        // Entered scene tree
	public override void _Ready() {
                base._Ready();
                displayName = "Room";
        }


        // Entered
        public override void Entered(Node2D activator) {
                
        }


        public override void Edit() {
                base.Edit();
                Button button = root.CreateButton("Enter Room");
                Callable callable = new Callable(this, "ChangeRoom");
                button.Connect("pressed", callable);
        }


        // Changes the current room to this one
        public void ChangeRoom() {
                root.ChangeCurrentRoom(this);
        }


        // Returns a list of all the blocks in this Room
        public AbstractBlock[,] GetCells() {
                return cells;
        }


        // Assigns the tiles
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


        // Place block in room grid from scene
        public void PlaceBlock(Vector2I pos, PackedScene blockScene) {
                if (!IsInstanceValid(GetBlockFromGrid(pos))) {
                        AbstractBlock instance = blockScene.Instantiate<AbstractBlock>();
                        SetBlockInGrid(pos, instance);
                }
        }


        // Place block in room grid from object
        public void PlaceBlock(Vector2I pos, AbstractBlock block) {
                if (!IsInstanceValid(GetBlockFromGrid(pos))) {
                        SetBlockInGrid(pos, block);
                }
        }


        // Remove block from room grid
        public void DeleteBlock(Vector2I pos) {
                if (IsInstanceValid(GetBlockFromGrid(pos))) {
                        RemoveBlockFromGrid(pos);
                }
        }


        // Remove block by reference
        public void DeleteBlock(AbstractBlock block) {
                if (IsInstanceValid(block)) {
                        Vector2I gridPos = block.GetGridPosition();
                        RemoveBlockFromGrid(gridPos);
                }
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
                Godot.Collections.Dictionary<string, string> dict = new();
                for (int i = 0; i < cells.GetLength(0); i++) {
                        for (int j = 0; j < cells.GetLength(1); j++) {
                                if (IsInstanceValid(cells[i, j])) {
                                        dict.Add("[" + i.ToString() + "," + j.ToString() + "]", cells[i, j].Save());
                                }
                        }
                }

                return Json.Stringify(new Godot.Collections.Dictionary{
                        ["Path"] = "res://Blocks/Triggerable/Room/room.tscn",             
                        ["Cells"] = dict
                });
        }


        public override void Load(Godot.Collections.Dictionary<string, Variant> data) {  
                base.Load(data);
                //data["Cells"]
        }
}
