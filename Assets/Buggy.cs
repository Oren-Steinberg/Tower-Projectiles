using UnityEngine;

public class Buggy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = transform.position.x;
        float z = transform.position.z;
        Rigidbody rb = GetComponent<Rigidbody>();
        Vector3 velocity = rb.linearVelocity;
        if(x <= -50 && velocity.x < 0 || x >= 50 && velocity.x > 0)
        {
            velocity.x *= -1;
        }
        if(z <= -50  && velocity.z < 0 || z >= 50 && velocity.z > 0)
        {
            velocity.z *= -1;
        }
        rb.linearVelocity = velocity;
    }

    public void Fire(Vector3 direction)
    {
        if (direction.sqrMagnitude == 0f)
        {
            Debug.LogError("Buggy direction must not be zero.", this);
            return;
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Buggy requires a Rigidbody component.", this);
            return;
        }

        rb.linearVelocity = direction;
    }

}
