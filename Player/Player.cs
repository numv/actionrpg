using Godot;
using System;

public class Player : KinematicBody2D
{
	public enum EState
	{
		move,
		roll,
		attack
	}

	[Export]
	public int Acceleration = 500;
	[Export]
	public int Friction = 550;
	[Export]
	public int SpeedMax = 100;
	[Export]
	public int SpeedRoll = 125;

	Vector2 velocity = Vector2.Zero;
	Vector2 roll_velocity = Vector2.Down;


	AnimationTree animation;
	AnimationNodeStateMachinePlayback animationState;
	EState State = EState.move;
	SwordHitbox swordHitbox;
	Stats stats;
	Hurtbox hurtbox;

	public override void _Ready()
	{
		stats = GetNode<Stats>("/root/PlayerStats");

		stats.Connect(nameof(Stats.NoHealth), this, nameof(OnNoHealth));

		hurtbox = GetNode<Hurtbox>(nameof(Hurtbox));
		animation = this.GetNode<AnimationTree>(nameof(AnimationTree));
		animationState = animation.Get("parameters/playback") as AnimationNodeStateMachinePlayback;
		animation.Active = true;

		swordHitbox = this.GetNode<SwordHitbox>("HitboxPivot/SwordHitbox");
		swordHitbox.KnockbackVector = roll_velocity;
	}

	public void OnNoHealth() => QueueFree();

	public override void _PhysicsProcess(float delta)
	{
		if (State == EState.move)
		{
			if (Input.IsActionJustPressed("player_attack"))
			{
				State = EState.attack;
			}
			if (Input.IsActionJustPressed("player_roll"))
			{
				State = EState.roll;
			}
		}

		switch (State)
		{
			case EState.move:
				MoveState(delta);
				break;
			case EState.roll:
				RollState(delta);
				break;
			case EState.attack:
				AttackState(delta);
				break;
		}
	}

	public void OnHurtBoxAreaEntered(Area2D area)
	{
		if (area is Hitbox hb)
			stats.Health -= hb.Damage;
		hurtbox.CreateHitEffect();
		hurtbox.Invincible = true;
	}

	public void AttackState(float delta)
	{
		animationState.Travel("attack");
		velocity = Vector2.Zero;
	}

	public void OnAttackAnimationFinished()
	{
		State = EState.move;
	}

	public void RollState(float delta)
	{
		animationState.Travel("roll");
		velocity = roll_velocity * SpeedRoll;
		Move();
	}

	public void OnRollAnimationFinished()
	{
		velocity = velocity / 1.5f;
		State = EState.move;
	}

	public void MoveState(float delta)
	{
		var inputVelocity = Vector2.Zero;
		inputVelocity.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
		inputVelocity.y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");
		inputVelocity = inputVelocity.Normalized();

		if (inputVelocity != Vector2.Zero)
		{
			roll_velocity = inputVelocity;
			swordHitbox.KnockbackVector = inputVelocity;

			animation.Set("parameters/idle/blend_position", inputVelocity);
			animation.Set("parameters/walk/blend_position", inputVelocity);
			animation.Set("parameters/attack/blend_position", inputVelocity);
			animation.Set("parameters/roll/blend_position", inputVelocity);
			velocity = velocity.MoveToward(inputVelocity * SpeedMax, Acceleration * delta);
			animationState.Travel("walk");
		}
		else
		{
			animationState.Travel("idle");
			velocity = velocity.MoveToward(Vector2.Zero, Friction * delta);
		}
		Move();
	}

	public void Move()
	{
		velocity = MoveAndSlide(velocity);
	}
}
