using Godot;
using System;

public class Hitbox : Area2D
{
	[Export]
	public int Damage { get; set; } = 1;
}
