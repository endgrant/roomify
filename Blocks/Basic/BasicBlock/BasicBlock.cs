using Godot;
using System;
using System.ComponentModel;

public partial class BasicBlock : AbstractBasic {
        // Entered scene tree
        public override void _Ready() {
                base._Ready();
                displayName = "Block";
        }
}
