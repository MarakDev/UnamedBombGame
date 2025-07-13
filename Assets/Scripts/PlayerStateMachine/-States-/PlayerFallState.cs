using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerState
{
    public PlayerFallState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
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
            if (IsGrounded())
            {
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
                    playerStateMachine.ChangeState(player.walkState);
                else
                    playerStateMachine.ChangeState(player.idleState);
            }
        }
    }

    public override void AnimationEnter()
    {
        //fall animation
        player.animator.Play("FALL");
    }
}
