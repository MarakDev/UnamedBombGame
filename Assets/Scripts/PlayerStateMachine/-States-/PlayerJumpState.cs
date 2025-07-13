using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }


    public override void EnterState()
    {
        base.EnterState();

        player.rb2D.velocity = Vector3.zero;

        horziontal_input = Input.GetAxisRaw("Horizontal");

        player.rb2D.AddForce(new Vector2(player.walk_speed * horziontal_input, player.jump_force), ForceMode2D.Impulse);

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
            if (player.rb2D.velocity.y <= 0 && IsGrounded())
            {
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
                    playerStateMachine.ChangeState(player.walkState);
                else
                    playerStateMachine.ChangeState(player.idleState);
            }

            if (player.rb2D.velocity.y < -7f && !IsGrounded())
                playerStateMachine.ChangeState(player.fallState);
        }
    }

    public override void AnimationEnter()
    {
        //jump animation
        player.animator.Play("JUMP");
    }
}
