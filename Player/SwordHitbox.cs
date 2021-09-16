using Godot;
using System;

public class SwordHitbox : Hitbox
{
	public Vector2 KnockbackVector { get; set; } = Vector2.Zero;
}
