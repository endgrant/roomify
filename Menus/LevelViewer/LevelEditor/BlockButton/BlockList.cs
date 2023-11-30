using Godot;
using System;

public partial class BlockList : Resource
{
        [Export(PropertyHint.File, "*.tscn")]
	public Godot.Collections.Array<string> blocks;


        public PackedScene GetBlockById(int id) {
                return GD.Load<PackedScene>("res://Blocks/" + blocks[id]);
        }
}
