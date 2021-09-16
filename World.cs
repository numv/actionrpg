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
	}
}
