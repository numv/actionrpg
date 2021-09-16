using Godot;
using System;

public class Camera2D : Godot.Camera2D
{

	public override void _Ready()
	{
		Position2D topLeft = GetNode<Position2D>("Limits/TopLeft");
		Position2D bottomRight = GetNode<Position2D>("Limits/BottomRight");

		LimitTop = Convert.ToInt32(topLeft.Position.y);
		LimitLeft = Convert.ToInt32(topLeft.Position.x);
		LimitBottom = Convert.ToInt32(bottomRight.Position.y);
		LimitRight = Convert.ToInt32(bottomRight.Position.x);
	}

}
