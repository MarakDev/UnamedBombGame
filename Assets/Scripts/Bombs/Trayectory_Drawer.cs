using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class Trayectory_Drawer : MonoBehaviour
{ 
    public LineRenderer lineRenderer; //"The marker will show where the projectile will hit"
    public float increment = 0.01f;
    public int maxPoints = 200;

    public void DrawTrajectory(Vector2 initialPosition, Vector2 initialVelocity, float currentDrag, float currentGravityScale)
    {
        bool deceleration_active = true;
        Vector2 velocity = initialVelocity;
        Vector2 position = initialPosition;
        Vector2 nextPosition;

        float hardDecThreshold = Mathf.Abs(initialVelocity.x * 0.5f);

        Vector2 currentGravity = Physics2D.gravity * currentGravityScale;

        UpdateLineRender(maxPoints, (0, position));

        for (int i = 1; i < maxPoints; i++)
        {
            if (deceleration_active == true)
            {
                if (velocity.y < 0 || Mathf.Abs(velocity.x) < hardDecThreshold)
                {
                    currentDrag = 0;

                    float direction = 1;
                    if (velocity.x < 0)
                        direction = -1;

                    float newVel = velocity.x - (hardDecThreshold * 0.015f * direction);
                    velocity = new Vector2(newVel, velocity.y);

                    if (Mathf.Abs(velocity.x) < hardDecThreshold * 0.25f)
                        deceleration_active = false;

                }
            }

            //Estimate velocity and update next predicted position
            velocity = CalculateNewVelocity(velocity, currentGravity, currentDrag, increment);

            nextPosition = position + velocity * increment;

            position = nextPosition;
            UpdateLineRender(maxPoints, (i, position)); //Unneccesary to set count here, but not harmful

        }
    }

    private Vector2 CalculateNewVelocity(Vector2 velocity, Vector2 gravity, float drag, float increment)
    {
        velocity += gravity * increment;
        velocity *= Mathf.Clamp01(1f - drag * increment);
        return velocity;
    }

    private void UpdateLineRender(int count, (int point, Vector2 pos) pointPos)
    {
        lineRenderer.positionCount = count;
        lineRenderer.SetPosition(pointPos.point, pointPos.pos);
    }

}
