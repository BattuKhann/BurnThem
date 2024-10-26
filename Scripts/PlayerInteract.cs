using Godot;
using System;

public partial class PlayerInteract : RayCast3D
{
	public override void _Process(double delta)
	{
		if(IsColliding()){
			var hit = GetCollider();
			if(hit is Interactable && Input.IsActionJustPressed("interact")){
				Interactable hitObj = (Interactable)hit;
				hitObj.interact();
			}
		}
	}
}
