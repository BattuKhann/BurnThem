using Godot;
using Godot.Collections;
using System;
using System.Collections;

public partial class Player : CharacterBody3D
{
	public float Speed;
	public const float WALK_SPEED = 5.0f;
	public const float SPRINT_SPEED = 8.0f;
	public const float JumpVelocity = 4.5f;

	private Node3D head;
	private Camera3D camera;
	private Area3D losObject;
	private Timer losTimer;
	private Control PlayerUiObject;
	private PlayerUi playerUi;

	private Array<Ghost> ghosts;

	private float mouse_sensitivity = 0.03f;
	private float twist_input;
	private float pitch_input;

	//bob variables
	const float BOB_FREQ = 2.0f;
	const float BOB_AMP = 0.08f;
	float t_bob = 0f;

	//game variables
	private double Stamina = 100;
	private int StaminaChange;
	private bool tired = false;

	public override void _Ready()
	{
		Array<Node> nodesInGroup = GetTree().GetNodesInGroup("ghost");
		ghosts = new Array<Ghost>();

		foreach (Node node in nodesInGroup)
		{
			if (node is Ghost ghost)
			{
				ghosts.Add(ghost);
			}
		}

		Input.MouseMode = Input.MouseModeEnum.Captured;
		head = GetNode<Node3D>("Head");
		camera = head.GetNode<Camera3D>("PlayerCamera");

		losObject = camera.GetNode<Area3D>("LineOfSightObject");
		losTimer = GetNode<Timer>("VisionTimer");

		PlayerUiObject = GetNode<Control>("PlayerUi");
		playerUi = (PlayerUi)PlayerUiObject;
	}
	public override void _PhysicsProcess(double delta)
	{
		if(Input.IsActionJustPressed("ui_cancel")){
			Input.MouseMode = Input.MouseModeEnum.Visible;
		}

		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor() && Stamina > 20)
		{
			velocity.Y = JumpVelocity;
			Stamina -= 20;
		}

		if(Input.IsActionPressed("sprint") && !tired){
			Speed = SPRINT_SPEED;
			StaminaChange = -30;
		} else {
			Speed = WALK_SPEED;
			StaminaChange = 5;
		}

		Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_back");
		Vector3 direction = (head.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if(IsOnFloor()){
			if (direction != Vector3.Zero)
			{
				velocity.X = direction.X * Speed;
				velocity.Z = direction.Z * Speed;
			}
			else
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
				velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
				StaminaChange = 10;
			}
		} else {
			velocity.X = Mathf.Lerp(velocity.X, direction.X * Speed, (float)delta * 3.0f);
			velocity.Z = Mathf.Lerp(velocity.Z, direction.Z * Speed, (float)delta * 3.0f);
		}

		Velocity = velocity;

		head.RotateY(twist_input);
		camera.RotateX(pitch_input);
		float newPitch = Mathf.Clamp(camera.Rotation.X, -0.7f, 1.05f);
		camera.Rotation = new Vector3(newPitch, camera.Rotation.Y, camera.Rotation.Z);
		
		twist_input = 0.0f;
		pitch_input = 0.0f;


		t_bob += (float)delta * velocity.Length() * (IsOnFloor() ? 1.0f : 0.0f);
		Transform3D lookTransform = camera.Transform;
		lookTransform.Origin = _headbob(t_bob);
		camera.Transform = lookTransform;

		Stamina += StaminaChange * delta;
		if(Stamina <= 0){
			Stamina = 0;
			tired = true;
		} else if(Stamina > 20)
			tired = false;

		playerUi.setStamina(Stamina);

		MoveAndSlide();
	}

	private Vector3 _headbob(float time)
	{
		Vector3 pos = Vector3.Zero;
		pos.Y = (float)Math.Sin(time * BOB_FREQ) * BOB_AMP;
		pos.X = (float)Math.Cos(time * BOB_FREQ / 2) * BOB_AMP;
		return pos;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if(@event is InputEventMouseMotion mouseMotion){
			if(Input.MouseMode == Input.MouseModeEnum.Captured){
				twist_input = - mouseMotion.Relative.X * mouse_sensitivity * 0.1f;
				pitch_input = - mouseMotion.Relative.Y * mouse_sensitivity * 0.1f;
			}
		}
	}

	public void _on_vision_timer_Timeout() {
		var overlaps = losObject.GetOverlappingBodies();
		var frozenGhosts = new Array<Ghost>();

		// check ghosts in collision sight
		if (overlaps.Count > 0) {
			for (int i = 0; i < overlaps.Count; i++) {
				if (overlaps[i].IsInGroup("ghost")) {
					Ghost ghost = (Ghost) overlaps[i];

					var space = GetWorld3D().DirectSpaceState;
					var query = PhysicsRayQueryParameters3D.Create(GlobalTransform.Origin, ghost.GlobalTransform.Origin);
					Dictionary result = space.IntersectRay(query);

					if (result.Count > 0) // Check if the ray hit something
					{
						var collider = (Node) result["collider"]; // Cast the collider to a Node
						if (collider != null && collider.IsInGroup("ghost"))
						{
							ghost.Freeze();
							frozenGhosts.Add(ghost);
						}
					}
				}
			}
		}

		foreach (Ghost g in ghosts) {
			if (!frozenGhosts.Contains(g)) {
				g.UnFreeze();
			}
		}
	}
}
