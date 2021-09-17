using Godot;
using System;

public class IntroScene : Node2D
{
	public override void _Ready()
	{
		AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), -20f);

		var sound = GetNode<AnimationTree>(nameof(AnimationTree));
		var animationState = sound.Get("parameters/playback") as AnimationNodeStateMachinePlayback;
		sound.Active = true;
		animationState.Start("PlayDeath");
	}

	public void OnRestartPressed()
	{
		if (!OS.IsDebugBuild())
			OS.WindowFullscreen = true;
		GetTree().ChangeScene("res://World.tscn");
	}
}
