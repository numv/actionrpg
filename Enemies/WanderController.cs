using Godot;
using System;

public class WanderController : Node2D
{

	Vector2 startPosition;
	public Vector2 targetPosition;
	static Random random = new Random();

	[Export]
	public int WanderRange = 32;

	Timer timer;

	public override void _Ready()
	{
		startPosition = GlobalPosition;
		targetPosition = GlobalPosition;

		timer = GetNode<Timer>(nameof(Timer));
	}

	public void UpdateTargetPosition()
	{
		var target_vec = new Vector2(random.Next(-WanderRange, WanderRange), random.Next(-WanderRange, WanderRange));
		targetPosition = startPosition + target_vec;
	}

	public void StartTimer(float duration)
	{
		timer.Start(duration);
	}

	public float GetTimeLeft()
	{
		return timer.TimeLeft;
	}

	public void OnTimeout()
	{
		UpdateTargetPosition();
	}
}
