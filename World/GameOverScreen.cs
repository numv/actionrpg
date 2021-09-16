using Godot;
using System;

public class GameOverScreen : Node2D
{
	public override void _Ready()
	{
		var sound = GetNode<AnimationTree>(nameof(AnimationTree));
		var animationState = sound.Get("parameters/playback") as AnimationNodeStateMachinePlayback;
		var rect = GetNode<TextureRect>("Control/VBox/HBox/DeathImage");
		sound.Active = true;
		animationState.Start("PlayDeath");

		var stats = GetNode<PlayerStats>("/root/PlayerStats");
		var tex = new ImageTexture();
		tex.CreateFromImage(stats.DeathImage);
		tex.SetSizeOverride(tex.GetSize().Clamped(100));
		rect.Texture = tex;
	}


	public void OnRestartPressed()
	{
		var stats = GetNode<PlayerStats>("/root/PlayerStats");
		stats.MaxHealth = stats.InitialMaxHealth;
		stats.Health = stats.MaxHealth;

		GetTree().ChangeScene("res://World.tscn");
	}
}
