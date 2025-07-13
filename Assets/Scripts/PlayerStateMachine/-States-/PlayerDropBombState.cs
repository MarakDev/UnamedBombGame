using System.Collections;
using System.Collections.Generic;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;

public class PlayerDropBombState : PlayerState
{
    public PlayerDropBombState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
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

        player.rb2D.velocity = Vector3.zero;

        if (player.has_beenLaunched)
        {
            player.has_beenLaunched = false;
            playerStateMachine.ChangeState(player.launchedState);
        }
        else
        {
            AnimatorStateInfo stateinfo = player.animator.GetCurrentAnimatorStateInfo(0);

            if (stateinfo.normalizedTime >= 0.9f)
                playerStateMachine.ChangeState(player.idleState);
        }
    }

    public override void AnimationEnter()
    {
        player.animator.Play("DROP_BOMB");
    }
}
