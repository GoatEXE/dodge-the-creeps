using Godot;
using System;

public partial class Main : Node
{
	[Export]
	public PackedScene MobScene {get; set; }

	private int _score;


	private void GameOver()
	{
		// Stop Timers
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();

		// Show Gameover HUD
		GetNode<HUD>("HUD").ShowGameOver();

		// Audio
		GetNode<AudioStreamPlayer>("Music").Stop();
		GetNode<AudioStreamPlayer>("DeathSound").Play();
	}

	private void NewGame()
	{
		// Reset Score
		_score = 0;

		// Create player and place in starting point
		var player = GetNode<Player>("Player");
		var startPosition = GetNode<Marker2D>("StartPosition");
		player.Start(startPosition.Position);

		// Set HUD Elements
		var hud = GetNode<HUD>("HUD");
		hud.UpdateScore(_score);
		hud.ShowMessage("Get Ready!");

		// Game start countdown
		GetNode<Timer>("StartTimer").Start();

		// Delete any mobs from previous game
		GetTree().CallGroup("mobs", Node.MethodName.QueueFree);

		// Start music
		GetNode<AudioStreamPlayer>("Music").Play();
	}


	// Timer Timeouts
		private void OnScoreTimerTimeout()
	{
		_score++;
		GetNode<HUD>("HUD").UpdateScore(_score);
	}

	private void OnStartTimerTimeout()
	{
		GetNode<Timer>("MobTimer").Start();
		GetNode<Timer>("ScoreTimer").Start();
	}

	private void OnMobTimerTimeout()
	{

		// Create a new instance of the Mob scene.
		object mobInstance = MobScene.Instantiate();
		if (mobInstance is Mob mob)
		{
			// Choose a random location on Path2D.
			var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
			mobSpawnLocation.ProgressRatio = GD.Randf();

			// Set the mob's direction perpendicular to the path direction.
			float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;

			// Set the mob's position to a random location.
			mob.Position = mobSpawnLocation.Position;

			// Add some randomness to the direction.
			direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
			mob.Rotation = direction;

			// Random velocity scaling with game duration.
			var velocity = new Vector2((float)GD.RandRange(150.0, 250.0) + _score, 0);
			mob.LinearVelocity = velocity.Rotated(direction);

			// Spawn the mob by adding it to the Main scene.
			AddChild(mob);
		}
		else
		{
			GD.PrintErr($"Instantiated object is not of type Mob. Actual type: {mobInstance.GetType().Name}");
		}
	}

}
