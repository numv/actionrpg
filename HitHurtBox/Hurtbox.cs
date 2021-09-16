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
				SetDeferred("monitoring", false);
				timer.Start(InvincibleDuration);
				EmitSignal(nameof(OnInvincibilityStarted));
			}
			else
			{
				SetDeferred("monitoring", true);
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
	public override void _Ready()
	{
		timer = GetNode<Timer>(nameof(Timer));
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
