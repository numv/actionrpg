using Godot;
using System;

public class IntroScene : Node2D
{
	public override void _Ready()
	{
		AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), -20f);

		var sound = GetNode<AnimationTree>(nameof(AnimationTree));
		var animationState = sound.Get("parameters/playback") as AnimationNodeStateMachinePlayback;
		var rect = GetNode<TextureRect>("Control/VBox/HBox/DeathImage");
		sound.Active = true;
		animationState.Start("PlayDeath");
	}

	public void OnRestartPressed()
	{
		OS.WindowFullscreen = true;
		GetTree().ChangeScene("res://World.tscn");
	}
}
