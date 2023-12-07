using Godot;
using System;

public partial class Player : CharacterBody2D {

	[ExportCategory("Attributes")]
	[Export(PropertyHint.Range, "0,10")] private int extraJumps = 0;
	[Export(PropertyHint.Range, "0, 2000")] private float maxSpeed = 750;
	[Export(PropertyHint.Range, "0, 100")] private float acceleration = 50;
	[Export(PropertyHint.Range, "0, 1")] private float friction = 0.8f;
	[Export(PropertyHint.Range, "0, 4000")] private float jumpForce = 1500;
	[Export(PropertyHint.Range, "0, 100")] private float gravityAccel = 75;
	[Export(PropertyHint.Range, "0, 4000")] private float gravityMax = 2000;
	private bool isTester = false;
	private bool enteredRoom = false;

	public void _physics_process(float delta) {
		// ignores player inputs if the pause screen is open and the player is not listed to be a test player
		if(!isTester && GetNode<PauseOverlay>("/root/PauseMenu").Visible == true)
			return;

		float xVel;
		float yVel;
		// gets the inputted movement direction from the player as a combination of inputs
		// Format: Input.GetAxis(negativeInput, positiveInput)
		float direction = Input.GetAxis("Move_Left", "Move_Right");
		
		// changes velocity of the character based on:
		// if the character is on the floor/ground 
		if(IsOnFloor()) {
			// finds the new speed in the x direction based on inputted direction and max speed
			// if there is no inputted direction, then applies friction instead
			if(direction == 0) // friction
				xVel = Velocity.X - (Velocity.X * 0.2f * friction);
			else               // acceleration
				xVel = Math.Clamp(Velocity.X + (acceleration * direction), -maxSpeed, maxSpeed);
			yVel = 0;
		}
		// if the character is in the air
		else {
			xVel = Math.Clamp(Velocity.X + (acceleration * direction * 0.5f), -maxSpeed, maxSpeed);
			yVel = Math.Clamp(Velocity.Y + gravityAccel, -gravityMax, gravityMax);
		}

		// changes velocity based on jumping
		if(Input.IsActionJustPressed("Jump")) {
			// if the player is on the floor/ground
			if(IsOnFloor())
				yVel = -jumpForce;
			// if the player is in the air and has extra jumps
			else if(extraJumps > 0) {
				yVel = -jumpForce * 0.75f;
				extraJumps--;
			}
		}

		// checks if the player just entered a new room to avoid a race situation
		if(enteredRoom) {
			enteredRoom = false;
			return;
		}
		// updates the character's velocity
		Velocity = new Vector2(xVel, yVel);
		MoveAndSlide();
		// Camp Position to sides of screen and kill if you fall out of screen
		//SetDeferred("GlobalPosition.X", Math.Clamp(GlobalPosition.X, ?, ?))
		//SetDeferred("GlobalPosition.X", Math.Clamp(GlobalPosition.X, ?, ?))
		//if(outsideOfScreen)
		//		Die();
	}

	public void SetIsTester(bool value) {
		isTester = value;
	}

	public void Die() {
		// Set player's global position to first spawn of level
		//GlobalPosition = GetNode<Spawn>("Spawn").GlobalPosition;
	}

	// sets the extra jumps for the player
	public void SetExtraJumps(int jumps) {
		if(extraJumps < jumps)
			extraJumps = jumps;
	}

    internal void EnteredRoom(Vector2 parentPos) {
		enteredRoom = true;
        Velocity = new Vector2(0, 0);
		MoveAndSlide();
		GlobalPosition = parentPos;
    }

    internal void Win() {
        
    }
}
