using Godot;
using System;

public partial class ComputerMonitor : Node3D
{
	public bool active = false;
	private Sprite3D screen;
	private ViewportTexture screenTexture;
	private SubViewport[] cameras = new SubViewport[5];
	private int currentCam = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		screen = (Sprite3D)GetNode("MeshInstance3D/Sprite3D");
		GD.Print(screen == null);
		screenTexture = (ViewportTexture)screen.Texture;
		GD.Print(screenTexture == null);
		GD.Print(GetTree().Root.Name);
		cameras[0] = (SubViewport)GetTree().Root.GetNode("MainLevel/SecurityCamera/VirtCam/SubViewport");
		GD.Print(cameras[0] == null);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(active){
			//GD.Print("HI");
			if(Input.IsActionJustPressed("ui_left") && currentCam > 0)
				currentCam -= 1;
			else if(Input.IsActionJustPressed("ui_right") && currentCam < 4)
				currentCam += 1;

			screenTexture.ViewportPath = cameras[0].GetPath();
		} else {
			screenTexture.ViewportPath = null;
		}
	}

	public void setActive(bool active){
		this.active = active;
	}
}
