using Godot;
using System;

public class Effect : AnimatedSprite
{
	public override void _Ready()
	{
		Frame = 0;
		Play("default");

		Connect("animation_finished", this, nameof(OnAnimationFinished));
	}

	public void OnAnimationFinished()
	{
		QueueFree();
	}
}
