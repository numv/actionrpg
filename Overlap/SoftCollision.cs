using Godot;
using System;

public class SoftCollision : Area2D
{
	public bool IsColliding()
	{
		var areas = GetOverlappingAreas();
		return areas.Count > 0;
	}

	public Vector2 GetPushVector()
	{
		var areas = GetOverlappingAreas();
		var push_vec = Vector2.Zero;
		if (areas.Count > 0)
		{
			var area = areas[0] as Area2D;
			push_vec = area.GlobalPosition.DirectionTo(GlobalPosition);
			push_vec = push_vec.Normalized();
		}
		return push_vec;
	}
}
