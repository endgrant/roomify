using Godot;
using System;
using System.Numerics;

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
	public static Godot.Vector2 respawnPoint;
	public static LevelPlayer levelPlayer;
	private AnimationPlayer animator;


        public override void _Ready() {
                base._Ready();
                        animator = GetNode<AnimationPlayer>("AnimationPlayer");
                        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", false);
        }


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
			//triggers animation
			if(yVel < 0) {
				if(xVel >= 0)
					animator.Play("Jump Right");
				else
					animator.Play("Jump Left");
			}
		}

		// checks if the player just entered a new room to avoid a race situation
		if(enteredRoom) {
			enteredRoom = false;
			return;
		}
		// updates the character's velocity
		Velocity = new Godot.Vector2(xVel, yVel);
		MoveAndSlide();
		// Clamp Position to sides of screen and kill if you fall out of screen
		GlobalPosition = new Godot.Vector2(Math.Clamp(GlobalPosition.X, 32, 1504), Math.Clamp(GlobalPosition.Y, 32, 950));
		if(GlobalPosition.Y >= 900)
			Die();
	}


	public void SetIsTester(bool value) {
		isTester = value;
		SetCollisionLayerValue(1, !isTester);
		SetCollisionLayerValue(5, isTester);
		for(int i = 2; i < 5; i++)
			SetCollisionMaskValue(i, !isTester);
		SetCollisionMaskValue(5, isTester);
	}


	public void Die() {
		EnteredRoom(respawnPoint);
	}


	// sets the extra jumps for the player
	public void SetExtraJumps(int jumps) {
		if(extraJumps < jumps)
			extraJumps = jumps;
	}


	public void EnteredRoom(Godot.Vector2 parentPos) {
		levelPlayer.StartTimer();
		enteredRoom = true;
		Velocity = new Godot.Vector2(0, 0);
		MoveAndSlide();
		respawnPoint = parentPos;
		GlobalPosition = respawnPoint;
	}


        public void Win() {
                animator.Play("Win");
        }


	public void StartedAnimation(string title) {
		SetPhysicsProcess(false);
	}


	public void FinishedAnimation(string title) {
		if(title.Equals("Win")) {
			levelPlayer.EndLevel();
		}
		else
			SetPhysicsProcess(true);
	}
}
