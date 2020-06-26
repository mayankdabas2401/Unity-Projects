using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleAI : MonoBehaviour
{
    [Header("Health")]
    [SerializeField]
    private int maxHealth = 3;
    [SerializeField]
    private int currentHealth;
    [SerializeField]
    private HealthBar healthBar;

    [Header("Flying")]
    [SerializeField]
    private float lookRadius = 10f;
    [SerializeField]
    private float distanceFromGround;
    [SerializeField]
    private float amplitude;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float hoverVelocity;

    [Header("Chasing")]
    [SerializeField]
    private float chasingRadius;
    [SerializeField]
    private float chasingSpeed;
    [SerializeField]
    private float chasingSmoothness;

    [Header("Attacking")]
    [SerializeField]
    private float attackingRange;

    [Header("Explosion")]
    [SerializeField]
    private GameObject explosionPrefab;

    private float angle;
    private float height;
    private bool facingRight;
    private bool isDead;
    private Player target;
    private Rigidbody2D eagleRigidbody;

    private void Awake()
    {
        isDead = false;
        eagleRigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        height = gameObject.GetComponent<BoxCollider2D>().size.y;
        facingRight = false;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Destroy gameobject
        if (isDead) Explosion();

        // Make the Enemy fly
        Fly();

        // Chase the target
        Chase();
    }

    private void Explosion()
    {
        GameObject explosionInstant = GameObject.Instantiate(explosionPrefab);
        explosionInstant.transform.position = gameObject.transform.position;
        explosionInstant.transform.rotation = gameObject.transform.rotation;
        Destroy(gameObject);
    }

    private void Fly()
    {
        // fly
        RaycastHit2D hit = Physics2D.Raycast(
                new Vector2(
                        transform.position.x,
                        transform.position.y - height / 2 - 0.1f
                    ),
                Vector2.down
            );

        if (hit.collider)
        {

            // Get the ground Position
            Vector3 targetPosition = hit.point;

            // Making the eagle to move upwards
            targetPosition = new Vector3(
                    targetPosition.x,
                    targetPosition.y + distanceFromGround,
                    targetPosition.z
                );

            // To and fro motion of eagle
            angle += speed * Time.deltaTime;
            float offset = Mathf.Cos(angle) * amplitude;

            targetPosition = new Vector3(
                    targetPosition.x,
                    targetPosition.y + offset,
                    targetPosition.z
                );

            // Move the gameobject to the target
            if(target != null && Vector3.Distance(transform.position, target.transform.position) <= attackingRange)
            {
                targetPosition = new Vector3(
                        targetPosition.x,
                        target.transform.position.y + 1.5f
                    );
            }

            // Apply the postion
            transform.position = new Vector3(
                    Mathf.Lerp(transform.position.x, targetPosition.x, hoverVelocity * Time.deltaTime),
                    Mathf.Lerp(transform.position.y, targetPosition.y, hoverVelocity * Time.deltaTime)
                );
        }
    }

    private void Chase()
    {
        Vector3 targetVelocity = Vector3.zero;
        if(target == null)
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(
                    new Vector2(
                        transform.position.x, 
                        transform.position.y),
                    chasingRadius/2,
                    Vector2.down
                );
            foreach(RaycastHit2D hit in hits)
            {
                if(hit.transform.GetComponent<Player>() != null)
                {
                    target = hit.transform.GetComponent<Player>();
                }
            }
        }

        // Check if target is out of range
        if (target != null && Vector3.Distance(transform.position, target.transform.position) > chasingRadius)
        {
            target = null;
        }

        // Chase the target
        if(target != null)
        {
            // Find the direction
            Vector3 direction = (target.transform.position - transform.position).normalized;

            // Making y axis equals to zero
            direction = new Vector3(direction.x, 0, direction.z);
            direction.Normalize();

            // Moving the eagle towards target
            targetVelocity = direction * chasingSpeed;
            Debug.Log(eagleRigidbody.velocity);

            if (eagleRigidbody.velocity.x > 0 && !facingRight)
            {
                Flip();
            }
            else if (eagleRigidbody.velocity.x < 0 && facingRight)
            {
                Flip();
            }

            eagleRigidbody.velocity = new Vector2(
                    Mathf.Lerp(
                            eagleRigidbody.velocity.x,
                            targetVelocity.x,
                            chasingSmoothness * Time.deltaTime
                        ),
                    Mathf.Lerp(
                            eagleRigidbody.velocity.y,
                            targetVelocity.y,
                            chasingSmoothness * Time.deltaTime
                        )
                );
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
    void Flip()
    {
        facingRight = !facingRight;
        gameObject.transform.Rotate(0f, 180f, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "FireBall")
        {
            Destroy(collision.gameObject);
            currentHealth--;
            healthBar.SetHealth(currentHealth);
            if(currentHealth <= 0) isDead = true;
        }
    }
}
