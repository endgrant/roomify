using Godot;
using System;

public abstract partial class AbstractBlock : Node2D
{
        private LevelEditor root;


	// Enter scene tree
	public override void _Ready() {
		base._Ready();
	}


	// Returns the texture of the sprite
	public Texture2D GetTexture() {
                Sprite2D sprite = GetNode<Sprite2D>("Sprite2D");
		return sprite.Texture;
	}


        // Edit block
	public virtual void Edit() {
                root = (LevelEditor)GetTree().Root.GetChild<Control>(0);
                root.SetEditedBlock(this);
        }
}
