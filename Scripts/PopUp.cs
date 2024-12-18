using Godot;
using System;

public partial class PopUp : Node3D
{
	public void setText(String text){
		Label textLabel = GetNode<CanvasLayer>("Canvas").GetNode<ColorRect>("ColorRect").GetNode<Label>("Label");
		textLabel.Text = text;
		//GetNode<CanvasLayer>("Canvas").GetNode<ColorRect>("ColorRect").Size = textLabel.Size;
	}

	public void setProgress(float prog){
		ProgressBar bar = GetNode<CanvasLayer>("Canvas").GetNode<ProgressBar>("DigProgress");
		bar.Value = prog;
	}
}
