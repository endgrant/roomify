using Godot;
using System;

public partial class Room : AbstractTriggerable
{
	private Room parentRoom;


        public override void Entered(Node2D activator)
        {
                throw new NotImplementedException();
        }


        public void SetParentRoom(Room parent) {
                parentRoom = parent;
        }


        public Room GetParentRoom() {
                return parentRoom;
        }
}
