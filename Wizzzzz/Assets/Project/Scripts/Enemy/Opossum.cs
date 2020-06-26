using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opossum : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed;
    public float walkDuration;

    [Header("Visuals")]
    public GameObject firePoint;
    public GameObject fireBall;
    public float shootDuration;
    public float shootCooldown;

    private Rigidbody2D opossumRigidbody;
    private float walkTimer;
    private bool movingRight;
    private bool facingRight;
    private bool isShooting;
    // Start is called before the first frame update
    void Start()
    {
        opossumRigidbody = gameObject.GetComponent<Rigidbody2D>();
        walkTimer = walkDuration;
        movingRight = true;
        facingRight = true;
        isShooting = false;
    }

    // Update is called once per frame
    void Update()
    {
        walkTimer -= Time.deltaTime;

        if(walkTimer <= 0.0f)
        {
            walkTimer = walkDuration;
            movingRight = !movingRight;
        }

        opossumRigidbody.velocity = new Vector2(
                walkSpeed * (movingRight ? 1 : -1),
                opossumRigidbody.velocity.y
            );

        if (opossumRigidbody.velocity.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (opossumRigidbody.velocity.x < 0 && facingRight)
        {
            Flip();
        }

        StartCoroutine(ShootRoutine());
    }

    IEnumerator ShootRoutine()
    {
        if(!isShooting)
        {
            isShooting = true;
            GameObject fireballInstance = GameObject.Instantiate(
                    fireBall,
                    firePoint.transform.position,
                    firePoint.transform.rotation
                );
            Destroy(fireballInstance, shootDuration);
            yield return new WaitForSeconds(shootCooldown);
            isShooting = false;
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        gameObject.transform.Rotate(0f, 180f, 0f);
    }
}
