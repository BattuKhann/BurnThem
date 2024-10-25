using Godot;
using System;

public partial class Ghost : RigidBody3D
{

	[Export]
	float speed = 2f;

	RigidBody3D player;
	private NavigationAgent3D agent;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var players = GetTree().GetNodesInGroup("player");

        // Ensure there is at least one player in the group
        if (players.Count > 0)
        {
            player = players[0] as RigidBody3D;
        }
        else
        {
            GD.PrintErr("Player not found in the 'player' group!");
        }

		agent = GetNode<NavigationAgent3D>("NavigationAgent3D");
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (player == null) {
			GD.Print("player is null");
            return;
		}

        // Calculate the direction towards the player
        Vector3 directionToPlayer = (player.GlobalTransform.Origin - GlobalTransform.Origin);
        
		float distanceToPlayer = directionToPlayer.Length();

        // If the enemy is too close to the player, stop moving
        if (distanceToPlayer <= 3)
        {
            // Stop movement if within stopping distance
            MoveAndCollide(Vector3.Zero);
            return;
        }

        // Normalize the direction vector
        directionToPlayer = directionToPlayer.Normalized();

        // Scale speed based on distance to avoid abrupt stopping
        float adjustedSpeed = Mathf.Lerp(0, speed, Mathf.Min(distanceToPlayer / 3, 1));

/*
		Vector3 separationForce = Vector3.Zero;

        // Check for nearby enemies to avoid collisions
        foreach (var enemyNode in GetTree().GetNodesInGroup("ghost"))
        {
            if (enemyNode != this) // Avoid self-check
            {
                // Calculate distance to other enemy
                Vector3 enemyPosition = ((RigidBody3D) enemyNode).GlobalTransform.Origin;
                Vector3 directionToEnemy = enemyPosition - GlobalTransform.Origin;
                float distanceToEnemy = directionToEnemy.Length();

                // If another enemy is within separation distance, apply separation force
                if (distanceToEnemy < 3)
                {
                    float strength = Mathf.Clamp(1.0f - (distanceToEnemy / 3), 0, 1);
                    Vector3 steerDirection = directionToEnemy.Normalized();
                    separationForce -= steerDirection * strength * 2; // Flee from the other enemy
                }
            }
        }

		if (separationForce.Length() > 0)
        {
            separationForce = separationForce.Normalized();
        }

		*/


        // Move towards the player with frame-rate independent speed
        Vector3 velocity = directionToPlayer * adjustedSpeed * (float)delta;

        // Apply the velocity using MoveAndSlide
        MoveAndCollide(velocity);

	}
}
