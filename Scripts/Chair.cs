using Godot;
using System;

public partial class Chair : StaticBody3D, Interactable
{
	public bool occupied = false;
	public bool leaving = false;
	private Camera3D chairCam;
	private Camera3D playerCam;
	private AnimationPlayer computerAnimation;
	private Node3D computer;
	private ComputerMonitor computerMonitor;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		chairCam = GetParent().GetNode<Camera3D>("DeskCamera");
		computer = GetParent().GetNode<Node3D>("ComputerMonitor");
		computerMonitor = (ComputerMonitor)computer;
		computerAnimation = GetParent().GetNode<AnimationPlayer>("AnimationPlayer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override async void _Process(double delta)
	{
		if(occupied && Input.IsActionJustPressed("interact2")){
			computerMonitor.active = false;
			computerAnimation.Play("close");
			leaving = true;
		}
		if(computerAnimation.CurrentAnimation != "close" && leaving){
			occupied = false;
			SetCollisionLayerValue(2, true);
			chairCam.ClearCurrent();
			playerCam.MakeCurrent();
			leaving = false;
		}
	}

	public void interact(Camera3D playerCamera)
	{
		computerMonitor.active = true;
		SetCollisionLayerValue(2, false);
		playerCam = playerCamera;
		playerCam.ClearCurrent();
		computerAnimation.Play("open");
		chairCam.MakeCurrent();
		occupied = true;
	}

	public string getInteractType()
	{
		return occupied ? "Stand" : "Sit";
	}
}
