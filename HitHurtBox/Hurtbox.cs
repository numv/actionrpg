using Godot;
using System;

public class Hurtbox : Area2D
{

	private bool _invincible = false;
	public bool Invincible
	{
		get => _invincible;
		set
		{
			_invincible = value;
			if (_invincible)
			{
				collisionShape.SetDeferred("disabled", true);
				timer.Start(InvincibleDuration);
				EmitSignal(nameof(OnInvincibilityStarted));
			}
			else
			{
				collisionShape.SetDeferred("disabled", false);
				EmitSignal(nameof(OnInvincibilityEnded));
			}
		}
	}

	[Export]
	public float InvincibleDuration = 0.5f;


	[Signal]
	delegate void OnInvincibilityStarted();

	[Signal]
	delegate void OnInvincibilityEnded();

	static PackedScene effect_hit = ResourceLoader.Load<PackedScene>("res://Effects/HitEffect.tscn");

	Timer timer;
	CollisionShape2D collisionShape;
	public override void _Ready()
	{
		timer = GetNode<Timer>(nameof(Timer));
		collisionShape = GetNode<CollisionShape2D>(nameof(CollisionShape2D));
	}

	public void CreateHitEffect()
	{
		var effect = effect_hit.Instance() as Node2D;

		var world = GetTree().CurrentScene;
		world.AddChild(effect);
		effect.GlobalPosition = GlobalPosition;
	}

	public void OnTimerTimeout()
	{
		Invincible = false;
		timer.Stop();
	}
}
