using Godot;
using System;

public class PlayerDetectionZone : Area2D
{

	public Player Player { get; set; }

	public bool CanSeePlayer()
	{
		return Player != null;
	}

	public void OnBodyEntered(Player body)
	{
		Player = body;
	}

	public void OnBodyExited(Node node)
	{
		Player = null;
	}
}
