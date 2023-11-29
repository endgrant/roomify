using Godot;
using System;

public abstract partial class AbstractBlock : Node2D
{
	private Sprite2D sprite;
	protected static Room currentRoom;


	// Enter scene tree
	public override void _Ready() {
		base._Ready();
		sprite = GetNode<Sprite2D>("Sprite2D");
	}


	// Returns the texture of the sprite
	public Texture2D GetTexture() {
		return sprite.Texture;
	}


	// Sets whether this block is ghosted or not
	public void SetGhosted(bool ghost) {
		if (ghost) {
			sprite.Modulate = new Color("#FFFFFF", 0.4F);
		} else {
			sprite.Modulate = new Color("#FFFFFF", 1.0F);
		}
	}


	public void Place(int x, int y) {
		// TODO: program place so that it places in the selected room at the specified location
	}


	// //public abstract void Edit();


	public static void SetRoom(Room newRoom) {
		currentRoom = newRoom;
	}  
}
