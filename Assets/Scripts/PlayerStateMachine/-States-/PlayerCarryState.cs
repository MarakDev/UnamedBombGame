
using UnityEngine;

public class PlayerCarryState : PlayerState
{
    public PlayerCarryState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
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

        //else
        //{
        horziontal_input = Input.GetAxisRaw("Horizontal");

        if (horziontal_input != 0)
            playerStateMachine.ChangeState(player.carryWalkState);

        if (Input.GetKeyDown(KeyCode.E))
            playerStateMachine.ChangeState(player.dropBombState);

        if (Input.GetKeyDown(KeyCode.Q))
            playerStateMachine.ChangeState(player.idleState);


        //}

    }

    public override void AnimationEnter()
    {
        player.animator.Play("IDLE_BOMB");
    }
}
