using Godot;
using Godot.Collections;
using System;

public partial class Ghost : CharacterBody3D
{
	private const float Speed = 2.0f;
	private const float accel = 10f;
	
	[Export]
	public bool frozen = false;
	[Export]
	public bool wandering = false;

	// init navigation agent
	private NavigationAgent3D navAgent;
	private Timer timer;
	[Export]
	public CharacterBody3D player;
	public float ChaseRange = 40.0f;
	private Vector3 wanderTarget;

    public override void _Ready()
    {
        navAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
		timer = GetNode<Timer>("Timer");
		RefreshPlayer();
		timer.Timeout += _on_timer_timeout;

		//var players = GetTree().GetNodesInGroup("player");

		/* Ensure there is at least one player in the group
		if (players.Count > 0)
		{
			player = players[0] as CharacterBody3D;
		}
		else
		{
			GD.PrintErr("Player not found in the 'player' group!");
		} */
    }

    public override void _PhysicsProcess(double delta)
	{
		// Check if player is not null and calculate the distance
		if (player != null)
		{
			float distanceToPlayer = GlobalPosition.DistanceTo(player.GlobalPosition);

			// Chase the player if within ChaseRange but only if close enough (within 10 units)
			if (!frozen && distanceToPlayer <= ChaseRange && distanceToPlayer <= 10.0f)
			{
				ChasePlayer(delta);
				timer.Stop(); // Stop the wandering timer while chasing
			}
			else if (!timer.IsStopped())
			{
				// Wander randomly if not chasing the player
				Wander(delta);
			}

			// OLD LOGIC
			/*
			float distanceToPlayer = GlobalPosition.DistanceTo(player.GlobalPosition);
			
			// Only move and rotate the ghost if it's not frozen and within the chase range
			if (!frozen && distanceToPlayer <= ChaseRange)
			{
				// Set the target position for the NavigationAgent
				navAgent.TargetPosition = player.GlobalPosition;
				
				// Calculate the direction to move in
				Vector3 dir = navAgent.GetNextPathPosition() - GlobalPosition;
				dir = dir.Normalized();
				
				// Rotate to face the player
				if (dir.Length() > 0)
				{
					// Calculate the target rotation using LookAt with only the Y-axis rotation
					Vector3 lookAtPosition = new Vector3(player.GlobalPosition.X, GlobalPosition.Y, player.GlobalPosition.Z);
					LookAt(lookAtPosition);
				}
				
				// Update the velocity to move toward the player
				Velocity = Velocity.Lerp(dir * Speed, (float)(accel * delta));
				MoveAndSlide();
			}
			*/
		}
	}

	private void ChasePlayer(double delta)
	{
		wandering = false;
		navAgent.TargetPosition = player.GlobalPosition;

		Vector3 dir = navAgent.GetNextPathPosition() - GlobalPosition;
		dir = dir.Normalized();

		if (dir.Length() > 0)
		{
			Vector3 lookAtPosition = new Vector3(player.GlobalPosition.X, GlobalPosition.Y, player.GlobalPosition.Z);
			LookAt(lookAtPosition);
		}

		Velocity = Velocity.Lerp(dir * Speed, (float)(accel * delta));
		MoveAndSlide();
	}

	private void Wander(double delta)
	{
		wandering = true;
		Vector3 dir = wanderTarget - GlobalPosition;
		if (dir.Length() < 1.0f)
		{
			// Choose a new wander target if close to the current one
			SetRandomWanderTarget();
		}

		dir = dir.Normalized();
		Velocity = Velocity.Lerp(dir * Speed, (float)(accel * delta));
		MoveAndSlide();
	}

	private void SetRandomWanderTarget()
	{
		// Pick a random direction within a small range
		Random random = new Random();
		float randomX = (float)(random.NextDouble() * 20 - 10);
		float randomZ = (float)(random.NextDouble() * 20 - 10);
		wanderTarget = GlobalPosition + new Vector3(randomX, 0, randomZ);
	}


	public void RefreshPlayer() {
		//var players = GetTree().GetNodesInGroup("player");

		/* Ensure there is at least one player in the group
		if (players.Count > 0)
		{
			player = players[0] as CharacterBody3D;
		}
		else
		{
			GD.PrintErr("Player not found in the 'player' group!");
		} */

		player = (CharacterBody3D) GetTree().Root.GetNode("MainLevel/Player");
	}

	public void _on_timer_timeout() {
		SetRandomWanderTarget();
		timer.Start(8.0f); // Restart the timer for 8 seconds
	}

	public void Freeze() {
		frozen = true;
	}

	public void UnFreeze() {
		frozen = false;
	}

	public bool isWandering() {
		return this.wandering;
	}
}
