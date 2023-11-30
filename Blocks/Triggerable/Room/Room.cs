using Godot;
using System;
using System.Collections.Generic;

public partial class Room : AbstractTriggerable
{
        private Node2D tiles;
	private Room parentRoom;
        private AbstractBlock[,] cells = new AbstractBlock[Constants.ROOM_WIDTH, Constants.ROOM_HEIGHT];
        private List<Room> rooms = new List<Room>();

        private Vector2I tileSize = new Vector2I(Constants.CELL_SIZE, Constants.CELL_SIZE);


        public Room() {}


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


        // Place block in room grid
        public void PlaceBlock(Vector2I pos, PackedScene blockScene) {
                if (!IsInstanceValid(GetBlockFromGrid(pos))) {
                        SetBlockInGrid(pos, blockScene);
                }
        }


        // Remove block from room grid
        public void DeleteBlock(Vector2I pos) {
                if (IsInstanceValid(GetBlockFromGrid(pos))) {
                        RemoveBlockFromGrid(pos);
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


        // Builds block in grid
        private void SetBlockInGrid(Vector2I pos, PackedScene scene) {
                AbstractBlock instance = scene.Instantiate<AbstractBlock>();
                cells[pos.X, pos.Y] = instance;
                instance.Position = ((pos + Vector2I.One) * tileSize) - (tileSize / 2);

                if (instance is Room) {
                        Room roomInstance = (Room)instance;
                        roomInstance.SetTiles(tiles);
                        roomInstance.SetParentRoom(this);
                }

                tiles.AddChild(instance);
        }


        // Removes block from grid
        private void RemoveBlockFromGrid(Vector2I pos) {
                AbstractBlock instance = GetBlockFromGrid(pos);
                if (IsInstanceValid(instance)) {
                        instance.QueueFree();
                        cells[pos.X, pos.Y] = null;
                }
        }
}
