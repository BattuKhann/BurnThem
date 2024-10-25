using Godot;
using System;

public partial class Grave : Node3D
{

	private PackedScene ghost;



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ghost = (PackedScene) GD.Load("res://Scenes/Ghost.tscn");

		SpawnEnemy(new Vector3(0, 10, 0));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public void SpawnEnemy(Vector3 position)
	{
		// Instance the enemy
		RigidBody3D enemy = (RigidBody3D) ghost.Instantiate();

		// Set the position of the enemy
		enemy.GlobalTransform = new Transform3D(Basis.Identity, position);

		// Add the enemy to the current scene
		AddChild(enemy);
	}
}
