using UnityEngine;

public class Trayectory_Explosion : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool deceleration_active = false;

    private float originalDrag = 0;
    private float deceleration_threshold = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalDrag = rb.drag;

    }

    public void StartExplosion(Vector2 explosion_force, Vector2 direction)
    {
        deceleration_active = true;
        rb.drag = originalDrag;

        //se añade la fuerza al objeto en radio de explosion
        rb.AddForce(explosion_force , ForceMode2D.Impulse);

        deceleration_threshold = Mathf.Abs(rb.velocity.x * 0.5f);

        float direction_angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //la direccion se comprueba si es hacia la izquierda o derecha
        direction_angle = (direction.x < 0) ? 1 : -1;

        float directionOfRotation = explosion_force.magnitude * direction_angle;

        rb.AddTorque(directionOfRotation);

    }


    private void FixedUpdate()
    {
        Deceleration();

    }

    private void Deceleration()
    {
        if (deceleration_active)
        {
            if (rb.velocity.y < 0 || Mathf.Abs(rb.velocity.x) < deceleration_threshold)
            {
                rb.drag = 0;

                float direction = 1;
                if(rb.velocity.x < 0)
                    direction = -1;

                float newVel = rb.velocity.x - (deceleration_threshold * 0.015f * direction);

                rb.velocity = new Vector2(newVel, rb.velocity.y);

                if (Mathf.Abs(rb.velocity.x) < deceleration_threshold * 0.25f) // un cuarto de la velocidad maxima de deceleracion para parar la deceleracion
                    deceleration_active = false;
            }
        }
    }


}
