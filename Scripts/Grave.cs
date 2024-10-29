using Godot;
using System;

public partial class Grave : Node3D, Interactable
{
	private PackedScene ghost;
	private Ghost enemy;
	private StaticBody3D graveBody;
	private Node3D deadman;
	private Node3D burntman;

	private Marker3D spawnPos;
	public bool opened = false;
	public bool burning = false;
	public bool occupied = true;
	public bool body = true;
	public float digProgress = 1.0f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ghost = (PackedScene) ResourceLoader.Load("res://Scenes/Ghost.tscn");
		spawnPos = GetNode<Marker3D>("Marker3D");
		graveBody = GetNode<StaticBody3D>("Body");
		deadman = GetNode<Node3D>("deadman");
		burntman = GetNode<Node3D>("deadman_burnt");

		//SpawnEnemy(spawnPos.GlobalPosition);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(opened){
			graveBody.Visible = false;
			deadman.Visible = true;
			if(!body){
				enemy.Free();
				deadman.Visible = false;
				burntman.Visible = false;
			}
		}
		
	}

	public void SpawnEnemy(Vector3 position)
	{
		//GD.Print("should spawn");
		// Instance the enemy
		enemy = ghost.Instantiate<Ghost>();

		// Set the position of the enemy
		enemy.GlobalTransform = new Transform3D(Basis.Identity, position);
		enemy.UnFreeze();

		// Add the enemy to the current scene
		GetTree().Root.AddChild(enemy);
		//AddChild(enemy);
		enemy.AddToGroup("ghost");
		//enemy._Ready();

		/*if (enemy != null) {
			enemy.UnFreeze();
			enemy.RefreshPlayer();
			//GD.Print("ghost isnt null");
		}*/

		//AddChild(enemy);

		var ghosts = GetTree().GetNodesInGroup("ghost");
		foreach (CharacterBody3D ghost in ghosts)
		{
			// Perform action on each ghost
			//GD.Print("Found ghost in group: ", ghost.Name);
		}

	}

	public void KillEnemy(){
		enemy.Free();
	}

	public void interact(Camera3D playerCam, double delta){
		GD.Print(digProgress);
		if(!opened){
			digProgress -= 0.1f * (float)delta;
			opened = digProgress <= 0;
		} else if (body){
			body = false;
			occupied = false;
			KillEnemy();
		}
	}

	public String getInteractType(){
		if(opened && body){
			return "Burn";
		} else {
			return "Dig";
		}
	}

	public float getProgress(){
		return (1 - digProgress) * 100;
	}
}
