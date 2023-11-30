using Godot;
using System;

public partial class BlockButton : VBoxContainer
{
        [Export]
        public BlockList blockList;

        private LevelEditor root;


        // Enter scene tree
        public override void _Ready() {
                base._Ready();
                root = (LevelEditor)GetTree().Root.GetChild<Control>(0);
        }


        // New option selected
        public void ItemSelected(int itemId) {
                root.SetCurrentBlock(blockList.GetAtlasId(), blockList.GetBlockById(itemId), itemId+1);
        }
}
