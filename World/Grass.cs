using Godot;
using System;

public class Grass : Node2D
{

	static PackedScene effect_grass = ResourceLoader.Load<PackedScene>("res://Effects/GrassEffect.tscn");
	public override void _Ready()
	{
	}

	public override void _Process(float delta)
	{
	}

	public void _on_Hurtbox_area_entered(Area2D area)
	{
		CreateGrassEffect();
		QueueFree();
	}

	public void CreateGrassEffect()
	{
		//var world = GetTree().CurrentScene;
		var effect = effect_grass.Instance() as Node2D;
		GetParent().AddChild(effect);
		effect.GlobalPosition = this.GlobalPosition;
	}
}
