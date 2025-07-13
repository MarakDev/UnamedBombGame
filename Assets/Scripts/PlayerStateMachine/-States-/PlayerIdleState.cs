using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }
    public override void EnterState()
    { 
        base.EnterState();

        player.rb2D.velocity = Vector3.zero;
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

            if (horziontal_input != 0)
                playerStateMachine.ChangeState(player.walkState);

            if (Input.GetKeyDown(KeyCode.E))
            {
                playerStateMachine.ChangeState(player.carryState);
            }

            //Desactivado el salto
            //if (Input.GetKeyDown(KeyCode.W) && IsGrounded())
            //{
            //    playerStateMachine.ChangeState(player.jumpState);
            //}

            CheckGroundedToFall();
        }

    }

    public override void AnimationEnter()
    {
        player.animator.Play("IDLE");
    }
}
