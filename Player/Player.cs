using Godot;
using System;

public partial class Player : CharacterBody2D {

	[ExportCategory("Attributes")]
	[Export(PropertyHint.Range, "0,10")] private int extraJumps = 0;
	[Export(PropertyHint.Range, "0, 2000")] private float maxSpeed = 750;
	[Export(PropertyHint.Range, "0, 100")] private float acceleration = 50;
	[Export(PropertyHint.Range, "0, 1")] private float friction = 0.2f;
	[Export(PropertyHint.Range, "0, 4000")] private float jumpForce = 1700;
	[Export(PropertyHint.Range, "0, 100")] private float gravityAccel = 75;
	[Export(PropertyHint.Range, "0, 4000")] private float gravityMax = 2000;

	public void _physics_process(float delta) {
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
			xVel = Math.Clamp(Velocity.X + (acceleration * direction * 0.25f), -maxSpeed, maxSpeed);
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

		// updates the character's velocity
		Velocity = new Vector2(xVel, yVel);
		MoveAndSlide();
	}

	public void Die() {
		// TODO: Add death sound/animation and queue free once 
		// 		 the death sound/animation has finished playing
		QueueFree();
	}

	// sets the extra jumps for the player
	public void SetExtraJumps(int jumps) {
		if(extraJumps < jumps)
			extraJumps = jumps;
	}
}
