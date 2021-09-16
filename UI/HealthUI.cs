using Godot;
using System;

public class HealthUI : Control
{
	private int _heart = 0;
	public int Heart
	{
		get => _heart;
		set
		{
			if (value.CompareTo(0) < 0)
				_heart = 0;
			else if (value.CompareTo(MaxHearts) > 0)
				_heart = MaxHearts;
			else
				_heart = value;

			HeartUIFull.RectSize = new Vector2(_heart * 15, HeartUIFull.RectSize.y);
		}
	}

	private int _maxHearts = 1;
	public int MaxHearts
	{
		get => _maxHearts; set
		{
			_maxHearts = value.CompareTo(1) < 0 ? 1 : value;
			HeartUIEmpty.RectSize = new Vector2(_maxHearts * 15, HeartUIEmpty.RectSize.y);
		}
	}

	TextureRect HeartUIFull;
	TextureRect HeartUIEmpty;

	public override void _Ready()
	{
		HeartUIFull = GetNode<TextureRect>(nameof(HeartUIFull));
		HeartUIEmpty = GetNode<TextureRect>(nameof(HeartUIEmpty));

		Stats playerStats = GetNode<Stats>("/root/PlayerStats");
		MaxHearts = playerStats.MaxHealth;
		Heart = playerStats.Health;
		playerStats.Connect(nameof(Stats.OnHealthChanged), this, nameof(OnHealthChanged));
		playerStats.Connect(nameof(Stats.OnMaxHealthChanged), this, nameof(OnMaxHealthChanged));
	}

	private void OnMaxHealthChanged(int health)
	{
		MaxHearts = health;
	}

	private void OnHealthChanged(int health)
	{
		Heart = health;
	}
}
