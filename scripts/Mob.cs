using Godot;
using System;

public partial class Mob : RigidBody2D
{

	public override void _Ready()
	{
		// Randomize the type of mob created per instance
		var animatedSprite2d = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		var mobTypes = animatedSprite2d.SpriteFrames.GetAnimationNames();
		animatedSprite2d.Play(mobTypes[GD.Randi() % mobTypes.Length]);
	}

	
	private void OnVisibleOnScreenNotifier2DScreenExited()
	{
		// Delete the mob when exiting the screen
		QueueFree();
	}
}
