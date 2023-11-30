using Godot;
using System;

public partial class BlockList : Resource
{
        [Export]
        public int atlasId;
        [Export(PropertyHint.File, "*.tscn")]
	public Godot.Collections.Array<string> blocks;


        public int GetAtlasId() {
                return atlasId;
        }


        public PackedScene GetBlockById(int id) {
                return GD.Load<PackedScene>("res://Blocks/" + blocks[id]);
        }
}
