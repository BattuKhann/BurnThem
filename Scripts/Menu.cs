using Godot;
using System;

public partial class Menu : Node2D
{
	private int HighScore = 0;
	private Button button;

	public override void _Ready()
	{
		button = GetNode<CanvasLayer>("CanvasLayer").GetNode<Button>("PlayButton");
		button.Pressed += StartGame;
	}

	public void StartGame(){
		GetTree().ChangeSceneToFile("res://Scenes/main_level.tscn");
	}

	public void UpdateScore(int Score){
		HighScore = Score > HighScore ? Score : HighScore;
		GetNode<CanvasLayer>("CanvasLayer").GetNode<Label>("HighScore").Text = "High Score: " + HighScore;
	}
}
