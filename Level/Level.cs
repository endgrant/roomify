using Godot;
using System;

public partial class Level : Node2D {
        public string levelName;
        public Room masterRoom;
	public Room currentRoom;
        

        public void Save() {
                string jsonString = masterRoom.Save();

                FileAccess file = FileAccess.Open(levelName + ".lvl", FileAccess.ModeFlags.Write);
                file.StoreLine(jsonString);
        }


        public void Load() {
                FileAccess file = FileAccess.Open(levelName + ".lvl", FileAccess.ModeFlags.Read);
                string jsonString = file.GetLine();
                Variant? data = Json.ParseString(jsonString);
                if (data == null) {
                        GD.Print("Parse Error");
                }

                Godot.Collections.Dictionary<string, Variant> dict = (Godot.Collections.Dictionary<string, Variant>)data;

                foreach (System.Collections.Generic.KeyValuePair<string, Godot.Variant> keyValuePair in dict) {
                        GD.Print(keyValuePair.Key, " ", keyValuePair.Value);
                }
        }
}
