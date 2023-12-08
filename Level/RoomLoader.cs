using Godot;


public class RoomLoader {
        protected PackedScene defaultRoomScene = GD.Load<PackedScene>("res://Blocks/Triggerable/Room/room.tscn");


        public Room GetRoom(Room newRoom, bool prev) {
                Room room = defaultRoomScene.Instantiate<Room>();
                Room prevRoom = RoomRequester.currentLevel.currentRoom;

                if(!GodotObject.IsInstanceValid(newRoom) || newRoom == null) {
                        return prevRoom;
                }

                prevRoom.SetRoomData((Godot.Collections.Dictionary<string, Variant>)Json.ParseString(prevRoom.Save()));

                foreach (AbstractBlock block in prevRoom.GetTiles().GetChildren()) {
                        if (GodotObject.IsInstanceValid(block)) {             
                                prevRoom.DeleteBlock(block);  
                        }
                }
                
                room.SetTiles(prevRoom.GetTiles());
                room.SetParentRoom(newRoom.GetParentRoom());
                room.parentPos = newRoom.parentPos;
                Godot.Collections.Dictionary<string, Variant> prevData = prevRoom.GetRoomData();
                Godot.Collections.Dictionary<string, Variant> newData = newRoom.GetRoomData();

                if (prev) {
                        Vector2I gridPos = prevRoom.parentPos;
                        string index = "[" + gridPos.X + "," + gridPos.Y + "]";
                        Godot.Collections.Dictionary<string, Variant> cells = (Godot.Collections.Dictionary<string, Variant>)newData["Cells"];
                        cells[index] = Json.Stringify(prevData); 
                }

                RoomRequester.currentLevel.parentRoomPos = room.parentPos;
                room.SetRoomData(newData);
                room.Load(room.GetRoomData());
                return room;
        }
}