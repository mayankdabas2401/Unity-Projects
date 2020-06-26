using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    [Header("Movements")]
    public float walkingSpeed;
    public float walkingDuration;

    private Rigidbody2D enemyRigidbody;
    private float walkingTimer;
    private bool walkingRight;
    // Start is called before the first frame update
    void Start()
    {
        enemyRigidbody = gameObject.GetComponent<Rigidbody2D>();
        walkingTimer = walkingDuration;
        walkingRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        walkingTimer -= Time.deltaTime;

        if(walkingTimer <= 0)
        {
            walkingTimer = walkingDuration;
            walkingRight = !walkingRight;
        }
        enemyRigidbody.velocity = new Vector2(
                walkingSpeed * (walkingRight ? 1 : -1),
                enemyRigidbody.velocity.y
            );

        if(walkingRight)
        {
            gameObject.transform.localScale = new Vector3(
                    1,
                    1,
                    1
                );
        }
        else if(!walkingRight)
        {
            gameObject.transform.localScale = new Vector3(
                    -1,
                    1,
                    1
                );
        }
    }
}
