using Godot;
using Godot.Collections;
using System;

public partial class Ghost : CharacterBody3D
{
	private const float Speed = 2.0f;
	private const float accel = 10f;
	
	[Export]
	public bool frozen = false;

	// init navigation agent
	private NavigationAgent3D navAgent;
	private CharacterBody3D player;

    public override void _Ready()
    {
        navAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");

		var players = GetTree().GetNodesInGroup("player");

		// Ensure there is at least one player in the group
		if (players.Count > 0)
		{
			player = players[0] as CharacterBody3D;
		}
		else
		{
			GD.PrintErr("Player not found in the 'player' group!");
		}
    }

    public override void _PhysicsProcess(double delta)
	{
		var dir = new Vector3();

		navAgent.TargetPosition = player.GlobalPosition;

		dir = navAgent.GetNextPathPosition() - GlobalPosition;
		dir = dir.Normalized();

		Velocity = Velocity.Lerp(dir * Speed, (float)(accel * delta));

		if (!frozen) {
			MoveAndSlide();
		}
	}

	public void Freeze() {
		frozen = true;
	}

	public void UnFreeze() {
		frozen = false;
	}
}
