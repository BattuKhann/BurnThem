using Godot;
using System;

public partial class Player_OLD : RigidBody3D
{
	float mouse_sensitivity = 0.005f;
	float twist_input = 0.00f;
	float pitch_input = 0.00f;
	float test_poo = 1;

	// test comment

	private Node3D _twistPivot;
	private Node3D _pitchPivot;
	private Camera3D camera3D;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;

		_twistPivot = GetNode<Node3D>("TwistPivot");
		_pitchPivot = _twistPivot.GetNode<Node3D>("PitchPivot");
		camera3D = GetNode<Camera3D>("PlayerCamera");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	[Export]
	public float Speed = 10.0f;

	public override void _Process(double delta)
	{
		Vector3 input = new Vector3();
		input.X = Input.GetAxis("move_left", "move_right");
		input.Z = Input.GetAxis("move_forward", "move_back");

		ApplyCentralForce(_twistPivot.Basis * input * 1200.0f * (float)delta);

		if(Input.IsActionJustPressed("ui_cancel")){
			Input.MouseMode = Input.MouseModeEnum.Visible;
		}
		/*
		_twistPivot.RotateY(twist_input);
		_pitchPivot.RotateX(pitch_input);
		float newPitch = Mathf.Clamp(_pitchPivot.Rotation.X, -0.5f, 0.5f);
		_pitchPivot.Rotation = new Vector3(newPitch, _pitchPivot.Rotation.Y, _pitchPivot.Rotation.Z);
		*/

		RotateY(twist_input);
		RotateX(pitch_input);
		float newPitch = Mathf.Clamp(Rotation.X, -0.5f, 0.5f);
		Rotation = new Vector3(newPitch, Rotation.Y, Rotation.Z);
		
		twist_input = 0.0f;
		pitch_input = 0.0f;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if(@event is InputEventMouseMotion mouseMotion){
			if(Input.MouseMode == Input.MouseModeEnum.Captured){
				twist_input = - mouseMotion.Relative.X * mouse_sensitivity;
				pitch_input = - mouseMotion.Relative.Y * mouse_sensitivity;
			}
		}
	}
}
