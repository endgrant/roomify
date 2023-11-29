using Godot;
using System;

public partial class BlockButton : VBoxContainer
{
        private PackedScene[] blocks = {
                GD.Load<PackedScene>("res://Blocks/Basic/BasicBlock/basic_block.tscn"),
                GD.Load<PackedScene>("res://Blocks/Basic/Toggleable/Red/red.tscn"),
                GD.Load<PackedScene>("res://Blocks/Basic/Toggleable/Blue/blue.tscn")
        };
        private LevelEditor root;


        // Enter scene tree
        public override void _Ready() {
                base._Ready();
                root = (LevelEditor)GetTree().Root.GetChild<Control>(0);
        }


        // New option selected
        public void ItemSelected(int itemId) {
                root.SetCurrentBlock(blocks[itemId]);
        }
}
