using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[ExportCategory("Attributes")]
	[Export(PropertyHint.Range, "0,10")] int extraJumps = 10;
	[Export(PropertyHint.Range, "0, 2000")] float maxSpeed = 750;
	[Export(PropertyHint.Range, "0, 100")] float acceleration = 50;
	[Export(PropertyHint.Range, "0, 4000")] float jumpForce = 2000;
	[Export(PropertyHint.Range, "0, 100")] float gravityAccel = 50;
	[Export(PropertyHint.Range, "0, 2000")] float gravityMax = 1000;

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
			xVel = Math.Clamp(Velocity.X + (acceleration * direction), -maxSpeed, maxSpeed);
			yVel = 0;
		}
		// if the character is in the air
		else {
			xVel = Math.Clamp(Velocity.X + (acceleration * direction * 0.1f), -maxSpeed, maxSpeed);
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
		GD.Print(Velocity.Y);
		MoveAndSlide();
	}

}
