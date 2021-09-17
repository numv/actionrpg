using Godot;
using System;

public class MobileControls : CanvasLayer
{

	[Signal]
	delegate void OnMoveVector(Vector2 pos);

	[Signal]
	delegate void OnAttack();
	[Signal]
	delegate void OnRoll();

	public override void _Ready()
	{
		if (!OS.HasTouchscreenUiHint())
			QueueFree();
	}

	public void JoystickOnMoveVector(Vector2 pos)
	{
		EmitSignal(nameof(OnMoveVector), pos);
	}

	public void OnAttackPressed()
	{
		EmitSignal(nameof(OnAttack));
	}

	public void OnRollPressed()
	{
		EmitSignal(nameof(OnRoll));
	}
}
