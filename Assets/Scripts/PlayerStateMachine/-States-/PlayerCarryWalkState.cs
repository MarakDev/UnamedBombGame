using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarryWalkState : PlayerState
{
    public PlayerCarryWalkState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
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

        CheckGroundedToFall();

        CheckLaunched();

        horziontal_input = Input.GetAxisRaw("Horizontal");

        if (horziontal_input == 0)
            playerStateMachine.ChangeState(player.carryState);

        if (Input.GetKeyDown(KeyCode.E))
            playerStateMachine.ChangeState(player.dropBombState);

        if (Input.GetKeyDown(KeyCode.Q))
            playerStateMachine.ChangeState(player.walkState);

        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        player.rb2D.velocity = new Vector2(horziontal_input * player.walk_speed * .75f, player.rb2D.velocity.y);

    }

    public override void ExitState()
    {
        base.ExitState();

    }


    public override void AnimationEnter()
    {
        player.animator.Play("WALK_BOMB");
    }
}
