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
	public int Friction { get; set; } = 80;
	[Export]
	public int Acceleration { get; set; } = 200;
	[Export]
	public int MaxSpeed { get; set; } = 50;

	static Random random = new Random();

	static PackedScene effect_death = ResourceLoader.Load<PackedScene>("res://Effects/EnemyDeathEffect.tscn");

	EState state = EState.idle;
	AnimatedSprite sprite;
	PlayerDetectionZone playerDetectionZone;
	Hurtbox hurtbox;
	SoftCollision softCollision;
	WanderController wanderController;
	AnimationPlayer BlinkAnimationPlayer;

	public override void _Ready()
	{
		stats = GetNode<Stats>(nameof(Stats));
		sprite = GetNode<AnimatedSprite>(nameof(Sprite));
		playerDetectionZone = GetNode<PlayerDetectionZone>(nameof(PlayerDetectionZone));
		hurtbox = GetNode<Hurtbox>(nameof(Hurtbox));
		softCollision = GetNode<SoftCollision>(nameof(SoftCollision));
		wanderController = GetNode<WanderController>(nameof(WanderController));

		BlinkAnimationPlayer = this.GetNode<AnimationPlayer>(nameof(BlinkAnimationPlayer));
	}

	public override void _PhysicsProcess(float delta)
	{
		knockback = knockback.MoveToward(Vector2.Zero, Friction * delta);
		knockback = MoveAndSlide(knockback);

		switch (state)
		{
			case EState.wander:
				DecideNextState();
				AccelerateTowards(wanderController.targetPosition, delta);
				if (GlobalPosition.DistanceTo(wanderController.targetPosition) <= 1)
				{
					DecideNextState(true);
				}
				SeekPlayer();
				break;
			case EState.chase:
				ChasePlayer(delta);
				break;

			case EState.idle:
			default:
				velocity = velocity.MoveToward(Vector2.Zero, Friction * delta);
				DecideNextState();
				SeekPlayer();
				break;
		}

		if (softCollision.IsColliding())
			velocity += softCollision.GetPushVector() * delta * Acceleration;
		velocity = MoveAndSlide(velocity);
	}

	public void AccelerateTowards(Vector2 pos, float delta)
	{
		var direction = GlobalPosition.DirectionTo(pos);
		velocity = velocity.MoveToward(direction * MaxSpeed, Acceleration * delta);
		sprite.FlipH = velocity.x < 0;
	}

	public void DecideNextState(bool force = false)
	{
		if (force || wanderController.GetTimeLeft() == 0)
		{
			state = GetRandomState(EState.idle, EState.wander);
			wanderController.StartTimer(random.Next(1, 3));
		}
	}

	public EState GetRandomState(params EState[] states)
	{
		return states[random.Next(0, states.Length)];
	}

	public void ChasePlayer(float delta)
	{
		if (!playerDetectionZone.CanSeePlayer())
			state = EState.idle;

		var player = playerDetectionZone.Player;
		if (player != null)
		{
			AccelerateTowards(player.GlobalPosition, delta);
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
		hurtbox.Invincible = true;
	}

	public void OnInvincibilityEnded()
	{
		BlinkAnimationPlayer.Play("stop");
	}

	public void OnInvincibilityStarted()
	{
		BlinkAnimationPlayer.Play("start");
	}

	public void OnStatsNoHealth()
	{
		CreateDeathEffect();
		QueueFree();
	}

	public void CreateDeathEffect()
	{
		var effect = effect_death.Instance<Node2D>();
		GetParent().AddChild(effect);
		effect.GlobalPosition = this.GlobalPosition;
	}
}
