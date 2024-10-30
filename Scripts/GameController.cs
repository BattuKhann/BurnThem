using Godot;
using Godot.Collections;
using System;

public partial class GameController : Node3D
{
	private int score = 0;
	private Array<Grave> unSpawned = new Array<Grave>();
	private Array<Grave> spawned = new Array<Grave>();
	private PackedScene menu;
	private Timer spawnTimer;

	public override void _Ready()
	{
		// Set up the Timer
		spawnTimer = new Timer();
		spawnTimer.WaitTime = 10.0f; // 10 seconds
		spawnTimer.OneShot = false;
		spawnTimer.Connect("timeout", new Callable(this, nameof(SpawnGhostAtRandomGrave)));
		AddChild(spawnTimer);
		spawnTimer.Start();

		// Initialize the graves list
		LoadGraves();
	}

	// Load all graves in the scene into the unSpawned list
	private void LoadGraves()
	{
		Node3D gravesRoot = GetParent().GetNode<Node3D>("NavigationRegion3D/graves1");
		
		foreach (Node child in gravesRoot.GetChildren())
		{
			if (child is Grave grave)
			{
				unSpawned.Add(grave);
			}
		}
	}

	// Function to spawn a ghost at a random grave
	private void SpawnGhostAtRandomGrave()
	{
		if (unSpawned.Count == 0)
		{
			GD.Print("No graves left to spawn ghosts.");
			return;
		}

		// Select a random grave from unSpawned
		int randomIndex = (int)GD.Randi() % unSpawned.Count;
		Grave selectedGrave = unSpawned[randomIndex];

		if (selectedGrave.occupied)
		{
			// Spawn a ghost at the selected grave's spawn position
			selectedGrave.SpawnEnemy();
			unSpawned.Remove(selectedGrave);
			spawned.Add(selectedGrave);
			GD.Print("Ghost spawned at a random grave.");
		}
	}

	public void AddScore()
	{
		score++;
	}

	public void GameOver()
	{
		// Implement your game over logic here
	}
}
