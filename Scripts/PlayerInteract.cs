using Godot;
using System;

public partial class PlayerInteract : RayCast3D
{
	private PopUp pop_up;
	private Camera3D playerCam;
	private AnimationPlayer animationPlayer;
	private Node3D Shovel;
	private Node3D Flamethrower;

	public override void _Ready()
	{
		pop_up = GetNode<PopUp>("PopUp");
		pop_up.GetNode<CanvasLayer>("Canvas").Visible = false;
		playerCam = (Camera3D)GetParent();
		animationPlayer = GetTree().Root.GetNode<CharacterBody3D>("MainLevel/Player").GetNode<AnimationPlayer>("AnimationPlayer");
		Shovel = GetTree().Root.GetNode<CharacterBody3D>("MainLevel/Player").GetNode<Node3D>("Head").GetNode<Node3D>("BestShovel");
		Flamethrower = GetTree().Root.GetNode<CharacterBody3D>("MainLevel/Player").GetNode<Node3D>("Head").GetNode<Camera3D>("PlayerCamera").GetNode<Node3D>("FlameThrower");
	}

	public override void _Process(double delta)
	{
		if(IsColliding()){
			var hit = GetCollider();
			if(hit is Interactable || ((Node3D)hit).GetParent() is Interactable){
				Interactable hitObj = (hit is Interactable) ? (Interactable)hit : (Interactable)(((Node3D)hit).GetParent());
				pop_up.setText(hitObj.getInteractType());
				pop_up.GetNode<CanvasLayer>("Canvas").Visible = true;

				if(hitObj.getInteractType() == "Dig"){
					pop_up.setProgress(((Grave)((Node3D)hit).GetParent()).getProgress());

					if (Input.IsActionPressed("interact")){
					hitObj.interact(playerCam, delta);

						if(hitObj.getInteractType() == "Dig"){
							Shovel.Visible = true;
							Flamethrower.Visible = false;
							animationPlayer.Play("Digging");
						} else if(hitObj.getInteractType() == "Burn"){
							Shovel.Visible = false;
							animationPlayer.Stop();
							Flamethrower.Visible = true;
							animationPlayer.Play("Burning");
						}
					} else {
						Shovel.Visible = false;
						//Flamethrower.Visible = false;
						animationPlayer.Stop();
					}
				} else {
					pop_up.GetNode<CanvasLayer>("Canvas").GetNode<ProgressBar>("DigProgress").Visible = false;
					
					if (Input.IsActionJustPressed("interact")){
						hitObj.interact(playerCam, delta);
					}
				}
			} else {
				pop_up.GetNode<CanvasLayer>("Canvas").Visible = false;
				Shovel.Visible = false;
				//Flamethrower.Visible = false;
				//animationPlayer.Stop();
			}
		} else {
			pop_up.GetNode<CanvasLayer>("Canvas").Visible = false;
			Shovel.Visible = false;
			//Flamethrower.Visible = false;
			//animationPlayer.Stop();
		}
	}
}
