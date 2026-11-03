using System;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    private bool hasBeenFired;
    void Start()
    {
        if (!hasBeenFired)
        {
            Fire(transform.forward);
        }
    }

    public void Fire(Vector3 direction)
    {
        if (direction.sqrMagnitude == 0f)
        {
            Debug.LogError("Projectile direction must not be zero.", this);
            return;
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Projectile requires a Rigidbody component.", this);
            return;
        }
        /* Constant Horizontal Launch Speed
        direction.y = 0;
        float distance = direction.magnitude;
        float time = distance / speed;
        float projectileBottom = transform.position.y - transform.localScale.y / 2;
        float verticalVelocity = 9.81f / 2f * time - projectileBottom / time;
        direction = direction.normalized * speed;
        direction.y = verticalVelocity;
        */

        // Constant Overall Launch Speed
        direction = GetLaunchVector3D(direction, speed, 9.81f, false);

        rb.linearVelocity = direction;
        hasBeenFired = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        Buggy buggy = collision.gameObject.GetComponentInParent<Buggy>();
        if (buggy != null)
        {
            Destroy(buggy.gameObject);
            GameObject scoreObject = GameObject.Find("Score");
            Score score = scoreObject.GetComponent<Score>();
            score.score++;
        }

        Destroy(gameObject);
    }

    private static Vector3 GetLaunchVector3D(Vector3 displacement, float v0, float g, bool highArc)
    {
        // Separate horizontal components from vertical (Y is assumed to be up)
        float dy = displacement.y;
        
        // 1. Calculate constants for our quadratic equation: A*(t^2)^2 + B*(t^2) + C = 0
        float A = 0.25f * g * g;
        float B = -(v0 * v0 - g * dy);         // Gravity acts downwards, so D dot G simplifies to -g * dy
        float C = displacement.sqrMagnitude; // Total 3D distance squared (dx^2 + dy^2 + dz^2)

        // 2. Check the discriminant to see if the target is reachable at this speed
        float discriminant = (B * B) - (4 * A * C);
        if (discriminant < 0)
        {
            Debug.LogError("Out of Range! : " + discriminant);
            return Vector3.zero; // Out of range! Target cannot be reached with speed v0.
        }

        // 3. Solve for t^2 (time of flight squared)
        float sqrtDisc = MathF.Sqrt(discriminant);
        
        // High arc uses the larger time value (+), Low arc uses the smaller time value (-)
        float t2 = highArc ? (-B + sqrtDisc) / (2 * A) : (-B - sqrtDisc) / (2 * A);
        
        if (t2 <= 0) return Vector3.zero; // Physical impossibility check

        float t = MathF.Sqrt(t2);

        // 4. Reconstruct the 3D launch vector components from the flight time
        // The horizontal components (X and Z) scale linearly with time
        float vx = displacement.x / t;
        float vy = (displacement.y / t) + (0.5f * g * t);
        float vz = displacement.z / t;

        return new Vector3(vx, vy, vz);
    }
}