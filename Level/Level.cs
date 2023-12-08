using Godot;
using System;
using System.Collections.Generic;

public partial class Level : Node2D {
        public string levelName;
	public Room currentRoom;
        

        public void Save(Godot.Collections.Dictionary<string, Variant> data) {
                string jsonString = Json.Stringify(data);
                levelName = levelName.TrimSuffix(".lvl");
                FileAccess file = FileAccess.Open(Constants.SAVE_DIR + "/" + levelName + ".lvl", FileAccess.ModeFlags.Write);
                file.StoreLine(jsonString);
                
                file.Close();
        }


        public void Load() {
                levelName = levelName.TrimSuffix(".lvl");
                FileAccess file = FileAccess.Open(Constants.SAVE_DIR + "/" + levelName + ".lvl", FileAccess.ModeFlags.Read);
                string jsonString = file.GetLine();
                Variant? data = Json.ParseString(jsonString);
                if (data == null) {
                        GD.Print("Parse Error");
                }

                
                Godot.Collections.Dictionary<string, Variant> dict = (Godot.Collections.Dictionary<string, Variant>)data;
                currentRoom.SetRoomData(dict);
                currentRoom.Load(dict);
                file.Close();
        }
}
