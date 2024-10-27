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
		// Check if the ghost is frozen; if so, stop movement
        if (frozen)
        {
            Velocity = Vector3.Zero; // Reset velocity when frozen
            return;
        }

        // Ensure the player is set
        if (player != null)
        {
            // Set the target position for the navigation agent
            navAgent.TargetPosition = player.GlobalPosition;

            // Calculate the direction towards the next path position
            Vector3 dir = navAgent.GetNextPathPosition() - GlobalPosition;
            dir = dir.Normalized();

            // Update velocity using linear interpolation for smooth movement
            Velocity = Velocity.Lerp(dir * Speed, (float)(accel * delta));

            // Move the ghost with the updated velocity
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
