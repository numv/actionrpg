using Godot;
using System;

public class PotionHealth : Node2D
{
	static PackedScene effect_healthup = ResourceLoader.Load<PackedScene>("res://Effects/HealthUpEffect.tscn");

	public override void _Ready()
	{
		var animation = GetNode<AnimationPlayer>(nameof(AnimationPlayer));
		animation.Play("idle");
	}

	public void OnBodyEntered(Node body)
	{
		if (body is Player player)
		{
			player.stats.Health++;
			CreateEffect();
			QueueFree();
		}
	}


	public void CreateEffect()
	{
		var effect = effect_healthup.Instance<Node2D>();
		GetTree().CurrentScene.AddChild(effect);
		effect.GlobalPosition = this.GlobalPosition;
	}
}
