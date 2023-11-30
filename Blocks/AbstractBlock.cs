using Godot;
using System;

public abstract partial class AbstractBlock : Node2D
{
        protected LevelEditor root;


	// Enter scene tree
	public override void _Ready() {
		base._Ready();
                root = (LevelEditor)GetTree().Root.GetChild<Control>(0);
	}


	// Returns the texture of the sprite
	public Texture2D GetTexture() {
                Sprite2D sprite = GetNode<Sprite2D>("Sprite2D");
		return sprite.Texture;
	}


        // Edit block
	public virtual void Edit() {
                root.SetEditedBlock(this);
        }
}
