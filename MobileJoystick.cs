using Godot;
using System;

public class MobileJoystick : CanvasLayer
{
	TouchScreenButton button;
	Vector2 centerBtn = new Vector2(32, 32);

	[Signal]
	delegate void OnMoveVector(Vector2 pos);

	public override void _Ready()
	{
		button = GetNode<TouchScreenButton>(nameof(TouchScreenButton));
	}

	public override void _Input(InputEvent e)
	{
		Vector2 pos = Vector2.Zero;
		if (e is InputEventScreenTouch t)
		{
			pos = t.Position;
		}
		else if (e is InputEventScreenDrag m)
		{
			pos = m.Position;
		}
		else
		{
			return;
		}


		if (!button.IsPressed())
		{
			if (moveVector != Vector2.Zero)
			{
				moveVector = Vector2.Zero;
				EmitSignal(nameof(OnMoveVector), moveVector);
			}
			return;
		}
		moveVector = CalculateMoveVector(pos);
		EmitSignal(nameof(OnMoveVector), moveVector);

	}


	Vector2 moveVector = Vector2.Zero;

	public Vector2 CalculateMoveVector(Vector2 pos)
	{
		var textureCenter = button.Position + centerBtn;
		return (pos - textureCenter).Normalized();
	}
}
