using Godot;
using Godot.Collections;
using System;

public partial class GameController : Node3D
{
	private int score = 0;
	Array<Grave> unSpawned;
	Array<Grave> Spawned;
	private PackedScene menu;

	public override void _Ready()
	{
		
	}

	public void addScore(){
		score++;
	}

	public void gameOver(){

	}
}
