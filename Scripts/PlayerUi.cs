using Godot;
using System;

public partial class PlayerUi : Control
{
	private ProgressBar StaminaBar;

	public override void _Ready(){
		StaminaBar = GetNode<ProgressBar>("StaminaBar");
	}

	public void setStamina(double var){
		StaminaBar.Value = var;
	}
}
