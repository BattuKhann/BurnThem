using Godot;
using System;

public partial class Grave : Node3D, Interactable
{
	private PackedScene ghost;
	private bool debug = false;

	private Marker3D spawnPos;
	public bool opened = false;
	public bool burning = false;
	public bool occupied = true;
	public bool body = true;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ghost = (PackedScene) ResourceLoader.Load("res://Scenes/Ghost.tscn");
		spawnPos = GetNode<Marker3D>("Marker3D");

		//SpawnEnemy(spawnPos.GlobalPosition);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// debug code to replace in _Ready()
		if (!debug) {
			debug = true;
			SpawnEnemy(spawnPos.GlobalPosition);
		}
	}

	public void SpawnEnemy(Vector3 position)
	{
		//GD.Print("should spawn");
		// Instance the enemy
		Ghost enemy = ghost.Instantiate<Ghost>();

		// Set the position of the enemy
		enemy.GlobalTransform = new Transform3D(Basis.Identity, position);
		enemy.UnFreeze();

		// Add the enemy to the current scene
		GetTree().Root.AddChild(enemy);
		//AddChild(enemy);
		enemy.AddToGroup("ghost");
		//enemy._Ready();

		if (enemy != null) {
			enemy.UnFreeze();
			enemy.RefreshPlayer();
			GD.Print("ghost isnt null");
		}

		//AddChild(enemy);

		var ghosts = GetTree().GetNodesInGroup("ghost");
		foreach (CharacterBody3D ghost in ghosts)
		{
			// Perform action on each ghost
			//GD.Print("Found ghost in group: ", ghost.Name);
		}

	}

	public void KillEnemy(){
		ghost.Free();
	}

	public void interact(Camera3D playerCam){
		
	}

	public String getInteractType(){
		if(opened && body){
			return "Burn";
		} else {
			return "Dig";
		}
	}
}
