using Godot;
using System;

public partial class Grave : Node3D, Interactable
{
	private PackedScene ghost = (PackedScene) ResourceLoader.Load("res://Scenes/Ghost.tscn");

	private Marker3D spawnPos;
	public bool opened = false;
	public bool burning = false;
	public bool occupied = true;
	public bool body = true;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		spawnPos = GetNode<Marker3D>("Marker3D");

		SpawnEnemy(spawnPos.GlobalPosition);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public void SpawnEnemy(Vector3 position)
	{
		// Instance the enemy
		CharacterBody3D enemy = ghost.Instantiate<CharacterBody3D>();

		// Set the position of the enemy
		enemy.GlobalTransform = new Transform3D(Basis.Identity, position);
		enemy.AddToGroup("ghost");

		// Add the enemy to the current scene
		AddChild(enemy);
	}

	public void KillEnemy(){
		ghost.Free();
	}

	public void interact(){
		
	}

	public String getInteractType(){
		if(opened && body){
			return "Burn";
		} else {
			return "Dig";
		}
	}
}
