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
        
        float distance = direction.magnitude;
        float time = distance / speed;
        float projectileBottom = transform.position.y - transform.localScale.y / 2;
        float verticalVelocity = 9.81f / 2f * time - projectileBottom / time;
        direction = direction.normalized * speed;
        direction.y = verticalVelocity;

        rb.linearVelocity = direction;
        hasBeenFired = true;
    }
}