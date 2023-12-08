using Godot;
using System;
[Tool]

public abstract partial class LevelViewer : AbstractMenu
{       
        protected PackedScene defaultLevelScene = GD.Load<PackedScene>("res://Level/level.tscn");
        protected PackedScene defaultRoomScene = GD.Load<PackedScene>("res://Blocks/Triggerable/Room/room.tscn");

        protected Level level;
        protected Node2D tiles;
        protected SubViewport viewport;

        protected Vector2I parentRoomPos = new Vector2I(-96, -96);


        public override void _Ready() {
                base._Ready();

                AbstractBlock.SetRoot(this);

                Room startingRoom = defaultRoomScene.Instantiate<Room>();
                level = defaultLevelScene.Instantiate<Level>();
                level.currentRoom = startingRoom;
                tiles = level.GetNode<Node2D>("Tiles");
                level.currentRoom.SetTiles(tiles);
                level.currentRoom.SetRoomData(null);
        }


        public abstract void NavPreviousRoom();


        public Vector2I GetParentRoomPos() {
                return parentRoomPos;
        }


        // Changes the current room
        public virtual void ChangeCurrentRoom(Room newRoom, bool prev) {
                if(newRoom == null) {
                        return;
                }
                Room prevRoom = level.currentRoom;

                prevRoom.SetRoomData((Godot.Collections.Dictionary<string, Variant>)Json.ParseString(prevRoom.Save()));

                foreach (AbstractBlock block in tiles.GetChildren()) {
                        if (IsInstanceValid(block)) {             
                                prevRoom.DeleteBlock(block);  
                        }
                }
                
                level.currentRoom = defaultRoomScene.Instantiate<Room>();
                level.currentRoom.SetTiles(tiles);
                level.currentRoom.SetParentRoom(newRoom.GetParentRoom());
                level.currentRoom.parentPos = newRoom.parentPos;
                Godot.Collections.Dictionary<string, Variant> prevData = prevRoom.GetRoomData();
                Godot.Collections.Dictionary<string, Variant> newData = newRoom.GetRoomData();

                if (prev) {
                        Vector2I gridPos = prevRoom.parentPos;
                        string index = "[" + gridPos.X + "," + gridPos.Y + "]";
                        Godot.Collections.Dictionary<string, Variant> cells = (Godot.Collections.Dictionary<string, Variant>)newData["Cells"];
                        cells[index] = Json.Stringify(prevData); 
                }

                parentRoomPos = level.currentRoom.parentPos;
                level.currentRoom.SetRoomData(newData);
                level.currentRoom.Load(level.currentRoom.GetRoomData());
        }


        public Node2D GetTiles() {
                return tiles;
        }
}
