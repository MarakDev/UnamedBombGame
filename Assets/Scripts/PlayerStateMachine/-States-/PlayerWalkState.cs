using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerState
{
    public PlayerWalkState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (player.has_beenLaunched)
        {
            player.has_beenLaunched = false;
            playerStateMachine.ChangeState(player.launchedState);
        }
        else
        {
            horziontal_input = Input.GetAxisRaw("Horizontal");

            if (horziontal_input == 0)
                playerStateMachine.ChangeState(player.idleState);

            if (Input.GetKeyDown(KeyCode.E))
            {
                playerStateMachine.ChangeState(player.carryWalkState);
            }

            //Desactivado el salto
            //if (Input.GetKeyDown(KeyCode.W) && IsGrounded())
            //{
            //    playerStateMachine.ChangeState(player.jumpState);
            //}

            CheckGroundedToFall();
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        player.rb2D.velocity = new Vector2(horziontal_input * player.walk_speed, player.rb2D.velocity.y);

    }

    public override void ExitState()
    {
        base.ExitState();

        
    }

    public override void AnimationEnter()
    {
        player.animator.Play("WALK");
    }

}
