using Godot;
using System;

public class PlayerStats : Stats
{
	[Export]
	public Image DeathImage { get; set; }

	[Export]
	public int InitialMaxHealth { get; set; } = 1;

	public PlayerStats()
	: base()
	{
		MaxHealth = InitialMaxHealth;
	}
}
