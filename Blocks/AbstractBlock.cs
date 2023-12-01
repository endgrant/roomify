using Godot;
using System;

public abstract partial class AbstractBlock : Node2D
{
        protected string displayName;
        protected LevelEditor root;


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


	// Returns the texture of the sprite
	public Texture2D GetTexture() {
                Sprite2D sprite = GetNode<Sprite2D>("Sprite2D");
		return sprite.Texture;
	}


        // Returns the name of this block
        public string GetDisplayName() {
                return displayName;
        }


        // Edit block
	public virtual void Edit() {
                root.SetEditedBlock(this);
        }
}
