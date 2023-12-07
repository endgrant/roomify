using Godot;
using System;

public partial class Level : Node2D {
        public string levelName;
        public Room masterRoom;
	public Room currentRoom;
        

        public void Save() {
                string jsonString = masterRoom.Save();
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
                masterRoom.SetRoomData(dict);
                masterRoom.Load(dict);

                file.Close();
        }
}
