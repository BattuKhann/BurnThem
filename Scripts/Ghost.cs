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
	[Export]
	public CharacterBody3D player;

    public override void _Ready()
    {
        navAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
		RefreshPlayer();

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
		//RefreshPlayer();

		if (player != null)
        {
            // Only move the ghost if it is not frozen
            if (!frozen)
            {
                navAgent.TargetPosition = player.GlobalPosition;
                Vector3 dir = navAgent.GetNextPathPosition() - GlobalPosition;
                dir = dir.Normalized();

                Velocity = Velocity.Lerp(dir * Speed, (float)(accel * delta));
                MoveAndSlide();
            }
        }
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

	public void Freeze() {
		frozen = true;
	}

	public void UnFreeze() {
		frozen = false;
	}
}
