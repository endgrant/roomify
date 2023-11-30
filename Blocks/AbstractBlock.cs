using Godot;
using System;

public abstract partial class AbstractBlock : Node2D
{
	protected static Room currentRoom;


	// Enter scene tree
	public override void _Ready() {
		base._Ready();
	}


	// Returns the texture of the sprite
	public Texture2D GetTexture() {
                Sprite2D sprite = GetNode<Sprite2D>("Sprite2D");
		return sprite.Texture;
	}

	public virtual void Edit() {

        }


	public static void SetRoom(Room newRoom) {
		currentRoom = newRoom;
	}  
}
