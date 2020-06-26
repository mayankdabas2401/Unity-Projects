using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigZagPath : MonoBehaviour
{
    public float walkingSpeed;
    public float walkingDuration;

    private Rigidbody2D objectRigidBody;
    private float walkingTimer;
    private bool walkingRight;

    // Start is called before the first frame update
    void Start()
    {
        objectRigidBody = gameObject.GetComponent<Rigidbody2D>();
        walkingTimer = walkingDuration;
        walkingRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Making timer work
        walkingTimer -= Time.deltaTime;
        if(walkingTimer <= 0.0f)
        {
            walkingTimer = walkingDuration;
            walkingRight = !walkingRight;
        }

        // Updating the velocity
        objectRigidBody.velocity = new Vector2(
                walkingSpeed * (walkingRight ? 1 : -1),
                objectRigidBody.velocity.y
            );
    }
}
