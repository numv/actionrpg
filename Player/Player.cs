using Godot;
using System;

public class Player : KinematicBody2D
{
	Vector2 velocity = Vector2.Zero;
	const int ACCELERATION = 500;
	const int FRICTION = 550;
	const int MAX_SPEED = 100;
	AnimationTree animation;
	AnimationNodeStateMachinePlayback animationState;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animation = this.GetNode<AnimationTree>("AnimationTree");
		animationState = animation.Get("parameters/playback") as AnimationNodeStateMachinePlayback;
		animation.Active = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		var inputVelocity = Vector2.Zero;
		inputVelocity.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
		inputVelocity.y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");
		inputVelocity = inputVelocity.Normalized();

		if (inputVelocity != Vector2.Zero)
		{
			animation.Set("parameters/idle/blend_position", inputVelocity);
			animation.Set("parameters/walk/blend_position", inputVelocity);
			velocity = velocity.MoveToward(inputVelocity * MAX_SPEED, ACCELERATION * delta);
			animationState.Travel("walk");
		}
		else
		{
			animationState.Travel("idle");
			velocity = velocity.MoveToward(Vector2.Zero, FRICTION * delta);
		}

		if (Input.IsActionJustPressed("player_dash"))
		{
			//isDashing = true;
		}

		velocity = MoveAndSlide(velocity);
	}
}
