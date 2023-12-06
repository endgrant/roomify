using Godot;
using System;

public partial class Level : Node2D {
        public string levelName;
        public Room masterRoom;
	public Room currentRoom;
        

        public void Save() {
                string jsonString = masterRoom.Save();

                FileAccess file = FileAccess.Open(Constants.SAVE_DIR + "/" + levelName + ".lvl", FileAccess.ModeFlags.Write);
                file.StoreLine(jsonString);
        }


        public void Load() {
                FileAccess file = FileAccess.Open(Constants.SAVE_DIR + "/" + levelName + ".lvl", FileAccess.ModeFlags.Read);
                string jsonString = file.GetLine();
                Variant? data = Json.ParseString(jsonString);
                if (data == null) {
                        GD.Print("Parse Error");
                }

                Godot.Collections.Dictionary<string, Variant> dict = (Godot.Collections.Dictionary<string, Variant>)data;
                Godot.Collections.Dictionary<string, string> cells = (
                        Godot.Collections.Dictionary<string, string>)dict["Cells"];

                foreach (System.Collections.Generic.KeyValuePair<string, string> keyValuePair in cells) {
                        Variant? internalData = Json.ParseString(keyValuePair.Value);
                        if (internalData == null) {
                                GD.Print("Parse Error");
                        }
                        Godot.Collections.Dictionary<string, Variant> internalDict = (Godot.Collections.Dictionary<string, Variant>)internalData;

                        PackedScene blockScene = GD.Load<PackedScene>((string)internalDict["Path"]);
                        AbstractBlock block = blockScene.Instantiate<AbstractBlock>();
                        block.Load(internalDict);
                        string[] stringVector = keyValuePair.Key.TrimPrefix("[").TrimSuffix("]").Split(",");
                        Vector2I gridPos = new Vector2I(Int32.Parse(stringVector[0]), Int32.Parse(stringVector[1]));
                        currentRoom.PlaceBlock(gridPos, blockScene);
                }
        }
}
