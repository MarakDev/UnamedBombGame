using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected Player player;
    protected PlayerStateMachine playerStateMachine;

    protected float horziontal_input;
    public PlayerState(Player player, PlayerStateMachine playerStateMachine)
    {
        this.player = player;
        this.playerStateMachine = playerStateMachine;
    }

    public virtual void EnterState() {

        AnimationEnter();
        player.smokeTrail.Stop();
        
    }
    public virtual void ExitState() { }
    public virtual void FrameUpdate(){ }
    public virtual void PhysicsUpdate() { }
    public virtual void AnimationTriggerEvent() { }

    public virtual void AnimationEnter() { }

    public bool IsGrounded()
    {
        if (Physics2D.BoxCast(player.transform.position, player.boxSize, 0, -player.transform.up, player.castDistance, player.groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool InSlider()
    {
        if (Physics2D.BoxCast(player.transform.position, player.boxSize * 0.5f, 0, -player.transform.up, player.castDistance, player.sliderLayer))
        {
            player.rb2D.AddForce(new Vector2(3 * player.rb2D.velocity.x, 0), ForceMode2D.Impulse);
            return true;
        }
        else
        {
            return false;
        }
    }

    protected void CheckGroundedToFall()
    {
        if (!IsGrounded())
        {
            playerStateMachine.ChangeState(player.fallState);
        }
    }

    protected void CheckLaunched()
    {
        if (player.has_beenLaunched)
        {
            player.has_beenLaunched = false;
            playerStateMachine.ChangeState(player.launchedState);
        }
    }
}
