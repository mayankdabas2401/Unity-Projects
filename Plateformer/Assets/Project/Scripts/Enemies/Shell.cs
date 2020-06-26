using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : Enemy
{
    public float movingSpeed;

    private Rigidbody2D shellRigidbody;
    private bool moving = false;
    private bool movingRight = true;
    private float width;
    
    // Start is called before the first frame update
    void Start()
    {
        shellRigidbody = gameObject.GetComponent<Rigidbody2D>();
        width = gameObject.GetComponent<CapsuleCollider2D>().size.x;
    }

    // Update is called once per frame
    void Update()
    {
        if(moving)
        {
            shellRigidbody.velocity = new Vector2(
                    movingSpeed * (movingRight ? 1: -1),
                    shellRigidbody.velocity.y
                );
        }
        if(movingRight)
        {
            RaycastHit2D rightHit = Physics2D.Raycast(
                    new Vector2(
                            transform.position.x + width / 2 + 0.01f,
                            transform.position.y
                        ),
                    Vector2.right,
                    0.1f
                );
            if(rightHit.collider != null)
            {
                if(rightHit.collider.gameObject.tag == "Enemy" && moving)
                {
                    Destroy(rightHit.collider.gameObject);
                }
                else if(rightHit.collider.gameObject.tag == "Player")
                {
                    Player player = rightHit.collider.gameObject.GetComponent<Player>();
                    player.HurtPlayer();
                }
                else
                {
                    movingRight = !movingRight;
                }
            }
        }
        else
        {
            RaycastHit2D leftHit = Physics2D.Raycast(
                    new Vector2(
                            transform.position.x - width / 2 - 0.01f,
                            transform.position.y
                        ),
                    Vector2.left,
                    0.1f
                );
            if(leftHit.collider != null)
            {
                if (leftHit.collider.gameObject.tag == "Enemy" && moving)
                {
                    Destroy(leftHit.collider.gameObject);
                }
                else if (leftHit.collider.gameObject.tag == "Player")
                {
                    Player player = leftHit.collider.gameObject.GetComponent<Player>();
                    player.HurtPlayer();
                }
                else
                {
                    movingRight = !movingRight;
                }
            }
        }
    }

    public override void OnHit(GameObject hitter)
    {
        moving = !moving;
        movingRight = hitter.transform.position.x < transform.position.x;
    }
}
