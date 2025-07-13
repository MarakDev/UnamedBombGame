using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class LaunchedStateGroundCheck : MonoBehaviour
{
    private Player player;
    private Vector2 velocity_bounce;

    private string currentState;

    private void Start()
    {
        player = GetComponent<Player>();

    }

    private void Update()
    {
        velocity_bounce = new Vector2(player.rb2D.velocity.x, player.rb2D.velocity.y);
        currentState = player.stateMachine.currentPlayerState.ToString();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentState == "PlayerLaunchedState" && collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Debug.Log("wall hit");
            player.rb2D.velocity = new Vector2(-velocity_bounce.x, velocity_bounce.y);
        }
        else if (currentState == "PlayerLaunchedState" && collision.gameObject.layer == LayerMask.NameToLayer("Celing"))
        {
            Debug.Log("celing hit");
            player.rb2D.velocity = new Vector2(velocity_bounce.x, -velocity_bounce.y);
        }
        else if (currentState == "PlayerLaunchedState" && collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            Debug.Log("floor hit");
            player.rb2D.velocity = new Vector2(velocity_bounce.x, -velocity_bounce.y);
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            player.is_Colision_Wall = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            player.is_Colision_Wall = false;
        }
    }


}
