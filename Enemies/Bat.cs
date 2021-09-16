using Godot;
using System;

public class Bat : KinematicBody2D
{
	public enum EState
	{
		idle,
		wander,
		chase,
	}

	Vector2 knockback = Vector2.Zero;
	Vector2 velocity = Vector2.Zero;

	Stats stats;


	[Export]
	public int Friction { get; set; } = 50;
	[Export]
	public int Acceleration { get; set; } = 200;
	[Export]
	public int MaxSpeed { get; set; } = 50;


	static PackedScene effect_death = ResourceLoader.Load<PackedScene>("res://Effects/EnemyDeathEffect.tscn");

	EState state = EState.idle;
	AnimatedSprite sprite;
	PlayerDetectionZone playerDetectionZone;
	Hurtbox hurtbox;

	public override void _Ready()
	{
		stats = GetNode<Stats>(nameof(Stats));
		sprite = GetNode<AnimatedSprite>(nameof(Sprite));
		playerDetectionZone = GetNode<PlayerDetectionZone>(nameof(PlayerDetectionZone));
		hurtbox = GetNode<Hurtbox>(nameof(Hurtbox));
	}

	public override void _PhysicsProcess(float delta)
	{
		knockback = knockback.MoveToward(Vector2.Zero, Friction * delta);
		knockback = MoveAndSlide(knockback);

		switch (state)
		{
			case EState.wander:
				break;
			case EState.chase:
				ChasePlayer(delta);
				break;

			case EState.idle:
			default:
				velocity = velocity.MoveToward(Vector2.Zero, Friction * delta);
				SeekPlayer();
				break;
		}

		velocity = MoveAndSlide(velocity);
	}

	public void ChasePlayer(float delta)
	{
		if (!playerDetectionZone.CanSeePlayer())
			state = EState.idle;

		var player = playerDetectionZone.Player;
		if (player != null)
		{
			var direction = (player.GlobalPosition - GlobalPosition).Normalized();
			velocity = velocity.MoveToward(direction * MaxSpeed, Acceleration * delta);
		}
		sprite.FlipH = velocity.x < 0;
	}

	public void SeekPlayer()
	{
		if (playerDetectionZone.CanSeePlayer())
			state = EState.chase;
	}

	public void _on_Hurtbox_area_entered(Area2D area)
	{
		if (area is SwordHitbox shb)
		{
			knockback = shb.KnockbackVector * 110;
		}
		if (area is Hitbox hb)
		{
			stats.Health -= hb.Damage;
		}
		hurtbox.CreateHitEffect();
	}

	public void OnStatsNoHealth()
	{
		CreateDeathEffect();
		QueueFree();
	}

	public void CreateDeathEffect()
	{
		var effect = effect_death.Instance() as Node2D;
		GetParent().AddChild(effect);
		effect.GlobalPosition = this.GlobalPosition;
	}
}
