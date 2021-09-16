using Godot;
using System;

public class Stats : Node
{
	[Export]
	private int _maxHealth;
	public int MaxHealth
	{
		get { return _maxHealth; }
		set
		{
			_maxHealth = value.CompareTo(1) < 0 ? 1 : value;
			if (Health > _maxHealth)
			{
				Health = _maxHealth;
			}
			EmitSignal(nameof(OnMaxHealthChanged), _maxHealth);
		}
	}


	private int _health;
	public int Health
	{
		get => _health;
		set
		{
			if (value.CompareTo(0) < 0)
				_health = 0;
			else if (value.CompareTo(MaxHealth) > 0)
				_health = MaxHealth;
			else
				_health = value;

			EmitSignal(nameof(OnHealthChanged), _health);
			if (_health <= 0)
			{
				EmitSignal(nameof(NoHealth));
			}
		}
	}

	[Signal]
	public delegate void NoHealth();

	[Signal]
	public delegate void OnHealthChanged(int health);
	[Signal]
	public delegate void OnMaxHealthChanged(int health);


	public override void _Ready()
	{
		Health = MaxHealth;
	}
}
