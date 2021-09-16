using Godot;
using System;

public class PlayerHurtSound : AudioStreamPlayer
{
	public void OnFinished()
	{
		QueueFree();
	}
}
