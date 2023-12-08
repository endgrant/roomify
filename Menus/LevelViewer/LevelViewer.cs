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
                return RoomRequester.currentLevel.parentRoomPos;
        }


        // Changes the current room
        public virtual void ChangeCurrentRoom(Room newRoom, bool prev) {
                level.ChangeRoom(newRoom, prev);
        }


        public Node2D GetTiles() {
                return tiles;
        }
}
