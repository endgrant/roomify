using Godot;
using System;

public abstract partial class AbstractBlock : Node2D
{
	protected static Room currentRoom;

	public void Place(int x, int y) {
		// TODO: program place so that it places in the selected room at the specified location
	}

	public abstract void Edit();

	public static void SetRoom(Room newRoom) {
		currentRoom = newRoom;
	}
}
