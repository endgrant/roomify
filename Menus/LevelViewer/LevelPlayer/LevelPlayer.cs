using Godot;
using System;

public partial class LevelPlayer : LevelViewer
{
        internal Timer timer;


        // Called when the node enters the scene tree for the first time.
        public override void _Ready() {
                timer = GetNode<Timer>("Timer");
        }


        internal void NavPreviousRoom() {
                throw new NotImplementedException();
        }
}
