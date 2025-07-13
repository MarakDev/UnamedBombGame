using UnityEngine;

public class PlayerLaunchedState : PlayerState
{
    private float aceleration = 1;
    private float aceleration_factor;
    Vector2 deceleration;
    public PlayerLaunchedState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        //player.rb2D.velocity = Vector3.zero;
        aceleration_factor = aceleration = player.explosive_power;
        //aceleration = Mathf.Clamp(aceleration, 1, 9);

        Vector2 final_force;

        //se añade la fuerza al objeto en radio de explosion
        if (player.is_Colision_Wall)//si el jugador esta pegado a una pared
            final_force = new Vector2(-player.explosive_force.x * aceleration, player.explosive_force.y * aceleration);
        else
            final_force = new Vector2(player.explosive_force.x * aceleration, player.explosive_force.y * aceleration);

        //player.rb2D.AddForce(final_force, ForceMode2D.Impulse);

        //hitstop
        int frameStop = (int)(player.explosive_power * 2);
        GameManager._GMinstance.FrameStop(frameStop);

        player.smokeTrail.Play();

    }
    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (player.has_beenLaunched)
        {
            player.has_beenLaunched = false;
            playerStateMachine.ChangeState(player.launchedState);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();


        ////se le añade a la velocidad el vector deceleracion
        //if(aceleration_factor * 2 > (aceleration - aceleration_factor))
        //{
        //    aceleration += aceleration_factor / 8;

        //    float deceleration_x = player.rb2D.velocity.x * (aceleration - aceleration_factor);
        //    float deceleration_y = player.rb2D.velocity.y * (aceleration - aceleration_factor);

        //    //vector que se multiplica a la fuerza del pesronaje en direccion contraria para frenarle
        //    deceleration = new Vector2(deceleration_x, deceleration_y);
            
        //    player.rb2D.AddForce(-deceleration, ForceMode2D.Force);
        //}
        //else
        //{
        //    playerStateMachine.ChangeState(player.fallState);
        //    player.smokeTrail.Stop();
        //}
    }

    public override void AnimationEnter()
    {
        player.animator.Play("FALL");
    }
}
