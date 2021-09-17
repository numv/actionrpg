using Godot;
using System;

public class World : Node2D
{

	YSort Bats;
	Label enemies;
	public override void _Ready()
	{
		AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), -20f);

		Bats = GetNode<YSort>("YSort/Bats");
		enemies = GetNode<Label>("CanvasLayer/Label");
	}

	public override void _Process(float delta)
	{
		var bats = Bats.GetChildren().Count;
		enemies.Text = "Bats: " + bats;

		if (bats == 0)
		{
			var stats = GetNode<PlayerStats>("/root/PlayerStats");
			stats.MaxHealth--;
			stats.Health = stats.MaxHealth;
			GetTree().ChangeScene("res://World.tscn");
		}
	}
}
