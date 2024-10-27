using Godot;
using System;

public partial class PlayerInteract : RayCast3D
{
	private PopUp pop_up;
	
	private Camera3D cam;

	public override void _Ready()
	{
		pop_up = GetNode<PopUp>("PopUp");
		pop_up.GetNode<CanvasLayer>("Canvas").Visible = false;

		cam = GetNode<Node3D>("Head").GetNode<Camera3D>("PlayerCamera");
	}

	public override void _Process(double delta)
	{
		if(IsColliding()){
			var hit = GetCollider();
			if(hit is Interactable){
				Interactable hitObj = (Interactable)hit;
				pop_up.setText(hitObj.getInteractType());
				pop_up.GetNode<CanvasLayer>("Canvas").Visible = true;
				
				if (Input.IsActionJustPressed("interact")){
					hitObj.interact(cam);
				}
			} else 
				 pop_up.GetNode<CanvasLayer>("Canvas").Visible = false;
		} else 
			pop_up.GetNode<CanvasLayer>("Canvas").Visible = false;
	}
}
