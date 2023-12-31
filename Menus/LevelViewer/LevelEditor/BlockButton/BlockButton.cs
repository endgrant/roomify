using Godot;
using System;
using System.Linq.Expressions;

public partial class BlockButton : VBoxContainer
{
        [Export]
        public BlockList blockList;

        private LevelEditor root;


        // Enter scene tree
        public override void _Ready() {
                base._Ready();
                root = GetTree().Root.GetNode<LevelEditor>("LevelEditor");
        }


        // New option selected
        public void ItemSelected(int itemId) {
                root.SetCurrentBlock(blockList.GetAtlasId(), blockList.GetBlockById(itemId), itemId+1, 
                        GetNode<OptionButton>("Option").GetItemIcon(itemId));
        }
}
