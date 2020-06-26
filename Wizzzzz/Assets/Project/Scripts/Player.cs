using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Delegates
    public delegate void OnDeadHandler();
    public delegate void OnCollectedHandler();
    public delegate void OnWinHandler();
    // Events
    public event OnDeadHandler OnDead;
    public event OnCollectedHandler OnCollected;
    public event OnWinHandler OnWin;

    [Header("Movements")]
    public float walkSpeed;
    public float jumpSpeed;
    [Header("Visuals")]
    public GameObject firePoint;
    public GameObject fireBall;
    [SerializeField]
    private GameObject itemCollectedPrefab;
    
    public float coolDown;
    public float destroyTimer;

    public bool Dead
    {
        get
        {
            return isDead;
        }
    }

    private Rigidbody2D playerRigidbody;
    private Animator playerAnimator;
    private float height;
    private bool facingRight;
    private bool isShooting;
    private bool isDead;
    private bool isCollected = false;

    // Start is called before the first frame update
    void Start()
    {
        // Getting reference of player's rigidbody
        playerRigidbody = gameObject.GetComponent<Rigidbody2D>();
        // Getting reference of player's animation
        playerAnimator = gameObject.GetComponent<Animator>();
        // Height of player
        height = gameObject.GetComponent<BoxCollider2D>().size.y;
        facingRight = true;
        isShooting = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Making player move
        playerRigidbody.velocity = new Vector2(
                walkSpeed * Input.GetAxis("Horizontal"),
                playerRigidbody.velocity.y
            );

        // Making player point to the right direction
        if (playerRigidbody.velocity.x > 0 && !facingRight)
        {
            Flip();
        }

        else if(playerRigidbody.velocity.x < 0 && facingRight)
        {
            Flip();
        }

        // Making player jump

        RaycastHit2D hit = Physics2D.Raycast(
                new Vector2(
                        transform.position.x,
                        transform.position.y - height/2 - 0.01f
                    ),
                Vector2.down,
                0.1f
            );
        if(hit.collider != null && Input.GetButtonDown("Jump"))
        {
            playerRigidbody.velocity = new Vector2(
                    playerRigidbody.velocity.x,
                    jumpSpeed
                );
        }

        // Making player shoot
        if(Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(ShootRoutine());
        }

        // Player's walk and jump animations
        playerAnimator.SetBool("Run", Mathf.Abs(playerRigidbody.velocity.x) > 0);
        playerAnimator.SetBool("Jump", Mathf.Abs(playerRigidbody.velocity.y) > 0);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Fall")
        {
            if (OnDead != null)
            {
                isDead = true;
                OnDead();
            }
        }

        if(collision.gameObject.tag == "Gems")
        {
            
            if(OnCollected != null)
            {
                OnCollected();
                GameObject itemCollectedInstant = GameObject.Instantiate(itemCollectedPrefab);
                itemCollectedInstant.transform.position = collision.gameObject.transform.position;
                itemCollectedInstant.transform.rotation = collision.gameObject.transform.rotation;
                Destroy(collision.gameObject);
            }
            
        }

        if(collision.gameObject.tag == "Win Trigger")
        {
            if(OnWin != null)
            {
                OnWin();
            }
        }
    }

    IEnumerator ShootRoutine()
    {
        if(!isShooting)
        {
            isShooting = true;

            GameObject fireballInstant = Instantiate(
                    fireBall,
                    firePoint.transform.position,
                    firePoint.transform.rotation
                );
            Destroy(fireballInstant, destroyTimer);
            yield return new WaitForSeconds(coolDown);

            isShooting = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
            if(OnDead != null)
            {
                isDead = true;
                OnDead();
            }
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        gameObject.transform.Rotate(0f, 180f, 0f);
       
    }
}
