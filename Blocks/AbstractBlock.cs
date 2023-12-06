using Godot;
using System;
using System.Reflection.Metadata;

public abstract partial class AbstractBlock : Node2D
{
        protected string displayName;
        protected LevelEditor root;
        protected int id;


	// Enter scene tree
	public override void _Ready() {
		base._Ready();

		try{
			root = (LevelEditor)GetTree().Root.GetChild<Control>(0);
		}
                catch(Exception ex) {
                        root = null;
                }
	}


        // Returns the name of this block
        public string GetDisplayName() {
                return displayName;
        }


        // Edit block
	public virtual void Edit() {
                root.SetEditedBlock(this);
        }


        // Returns the grid position of this block
        public Vector2I GetGridPosition() {
                return (Vector2I)(Position / new Vector2(Constants.CELL_SIZE, Constants.CELL_SIZE));
        }


        // Decompiles the block into a json string
        public abstract string Save();


        // Compiles the block from a json string
        public abstract void Load(string data);
}
