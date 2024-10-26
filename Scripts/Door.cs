using Godot;
using System;

public partial class Door : StaticBody3D, Interactable
{
	bool open = false;
	bool interactable = true;
	[Export] AnimationPlayer animationPlayer;

	public override void _Ready()
	{
		//animationPlayer.Play("reset");
	}
	public async void interact(){
		if(interactable){
			interactable = false;
			if(open)
				animationPlayer.Play("close");
			else
				animationPlayer.Play("open");
			open = !open;
			Timer timer = new Timer();
			timer.WaitTime = 1.0f; // Set wait time to 1 second
			timer.OneShot = true; // Set it to only run once

			AddChild(timer);

			timer.Start();

			await ToSignal(timer, "timeout");

			interactable = true;

			timer.QueueFree();
		}
	}
}
