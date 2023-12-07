using Godot;
using System;
using System.Reflection.Metadata;

public abstract partial class AbstractBlock : Node2D {
        protected string displayName;
        protected static LevelViewer root;
        protected int id;


	// Enter scene tree
	public override void _Ready() {
		base._Ready();
	}


        // Returns the name of this block
        public string GetDisplayName() {
                return displayName;
        }


        // Edit block
	public virtual void Edit() {
                ((LevelEditor)root).SetEditedBlock(this);
        }


        // Returns the grid position of this block
        public Vector2I GetGridPosition() {
                return (Vector2I)(Position / new Vector2(Constants.CELL_SIZE, Constants.CELL_SIZE));
        }


        // Decompiles the block into a json string
        public abstract string Save();


        // Compiles the block from a json string
        public virtual void Load(Godot.Collections.Dictionary<string, Variant> data) {}

        public static void SetRoot(LevelViewer viewer) {
                root = viewer;
        }
}
