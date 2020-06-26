using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Delegates
    public delegate void CoinCollectHandler();
    public delegate void WinHandler();
    public delegate void LoseHandler();
    // Events
    public event CoinCollectHandler OnCoinCollected;
    public event WinHandler OnWin;
    public event LoseHandler OnLose;
    
    [Header("Movement")]
    public float powerupScale = 1.5f;
    public float walkingSpeed = 3.0f;
    public float jumpingSpeed = 5.0f;
    
    [Header("Visuals")]
    public GameObject container;
    public GameObject bullet;
    public GameObject bulletPrefab;
    public GameObject firePoint;

    [Header("Audio")]
    public AudioSource destroyAudio;
    public AudioSource powerupAudio;
    public AudioSource jumpAudio;
    public AudioSource coinAudio;
    public AudioSource hurtAudio;

    [Header("Timer")]
    public float destroyTimer;

    public bool Dead
    {
        get
        {
            return isDead;
        }
    }

    private Rigidbody2D playerRigidbody;
    private float height;
    private float width;
    private bool isDead;
    private bool hasPowerup;
    private bool isInvincible;
    private bool controllable;
    private Animator playerAnimator;
    private bool hasBulletPowerup;
    private bool facingRight;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = gameObject.GetComponent<Rigidbody2D>();
        playerAnimator = gameObject.GetComponent<Animator>();
        height = gameObject.GetComponent<CapsuleCollider2D>().size.y;
        width = gameObject.GetComponent<CapsuleCollider2D>().size.x;
        controllable = true;
        hasBulletPowerup = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDead && controllable)
        {
            // Make the player move horizontally
            playerRigidbody.velocity = new Vector2(
                    Input.GetAxis("Horizontal") * walkingSpeed,
                    playerRigidbody.velocity.y
                );
            // Make the player point in the right direction

            if(Input.GetAxis("Horizontal") > 0)
            {
                container.transform.localScale = new Vector3(
                        1,
                        1,
                        1
                    );
               facingRight = true;
            }
            else if(Input.GetAxis("Horizontal") < 0)
            {
                container.transform.localScale = new Vector3(
                        -1,
                        1,
                        1
                    );
                facingRight = false;
            }

            // Player's walk anim
            playerAnimator.SetBool("Run", Mathf.Abs(playerRigidbody.velocity.x) > 0);

            RaycastHit2D hit = Physics2D.Raycast(
                    new Vector2(
                            transform.position.x,
                            transform.position.y - height / 2 - 0.01f
                        ),
                    Vector2.down,
                    0.02f
                );

            // Make the player jump
            if (hit.collider != null && Input.GetAxis("Jump") > 0)
            {
                playerRigidbody.velocity = new Vector2(
                    playerRigidbody.velocity.x,
                    jumpingSpeed
                );

                jumpAudio.Play();
            }

            // Shooting Logic
            if (Input.GetButtonDown("Fire1") && hasBulletPowerup == true)
            {
                
                if(facingRight == true)
                {
                    Debug.Log(facingRight);
                    GameObject bulletInstant = GameObject.Instantiate(bulletPrefab);
                    bulletInstant.transform.position = firePoint.transform.position;
                    bullet.GetComponent<SpriteRenderer>().flipX = false;
                    Destroy(bulletInstant, destroyTimer);
                }
                else if(facingRight == false)
                {
                    Debug.Log(facingRight);
                    GameObject bulletInstant = GameObject.Instantiate(bulletPrefab);
                    bulletInstant.transform.position = firePoint.transform.position;
                    bullet.GetComponent<SpriteRenderer>().flipX = true;
                    Destroy(bulletInstant, destroyTimer);
                }
                
              
            }

            // Player's Jump animation
            playerAnimator.SetBool("Jump", hit.collider == null);
        }
    }

    // OnTriggerEnter2D called once enter a collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Collect coin
        if(collision.gameObject.tag == "Coin")
        {
            // Increase coin counter
            AddCoins();
            // Destroy coin game object
            Destroy(collision.gameObject);
            coinAudio.Play();
        }
        // Collect powerups
        if(collision.gameObject.tag == "Powerup")
        {
            hasPowerup = true;
            powerupAudio.Play();
            // Increase the player size
            transform.localScale = new Vector3(
                    1.0f,
                    powerupScale,
                    1.0f
                );
            // Destroy powerup game object
            Destroy(collision.gameObject);
            height *= powerupScale;
        }
        if(collision.gameObject.tag == "BulletPowerup")
        {
            hasBulletPowerup = true;
            powerupAudio.Play();
            Destroy(collision.gameObject);
        }

        // Level complete
        if(collision.gameObject.tag == "FinishLine")
        {
            powerupAudio.Play();
            controllable = false;
            if(OnWin != null)
            {
                OnWin();
            }
        }
    }

    public void AddCoins()
    {
        if (OnCoinCollected != null)
        {
            OnCoinCollected();
        }
    }

    // OnCollisionEnter2D called once enter a collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Enemy collision
        if(collision.gameObject.tag == "Enemy")
        {
            RaycastHit2D leftHit = Physics2D.Raycast(
                    new Vector2(
                            transform.position.x - width / 2,
                            transform.position.y - height / 2 - 0.01f
                        ),
                    Vector2.down,
                    0.02f
                );
            RaycastHit2D rightHit = Physics2D.Raycast(
                    new Vector2(
                            transform.position.x + width / 2,
                            transform.position.y - height / 2 - 0.01f
                        ),
                    Vector2.down,
                    0.02f
                );
            RaycastHit2D centerHit = Physics2D.Raycast(
                    new Vector2(
                            transform.position.x,
                            transform.position.y - height / 2 - 0.01f
                        ),
                    Vector2.down,
                    0.02f
                );

            if ((centerHit.collider != null && centerHit.collider.gameObject == collision.gameObject) || 
                (leftHit.collider != null && leftHit.collider.gameObject == collision.gameObject) || 
                (rightHit.collider != null && rightHit.collider.gameObject == collision.gameObject))
            {
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();

                if(enemy.hittable)
                {
                    enemy.OnHit(gameObject);

                    playerRigidbody.velocity = new Vector2(
                            0,
                            jumpingSpeed
                        );
                }
                else
                {
                    HurtPlayer();
                }
            }
            else
            {
                HurtPlayer();
            }
            
        }

        if(collision.gameObject.tag == "Block" && collision.relativeVelocity.y < 0.0f)
        {
            RaycastHit2D leftHit = Physics2D.Raycast(
                    new Vector2(
                            transform.position.x - width / 2,
                            transform.position.y + height / 2 + 0.01f
                        ),
                    Vector2.up,
                    0.02f
                );
            RaycastHit2D rightHit = Physics2D.Raycast(
                    new Vector2(
                            transform.position.x + width / 2,
                            transform.position.y + height / 2 + 0.01f
                        ),
                    Vector2.up,
                    0.02f
                );
            RaycastHit2D centerHit = Physics2D.Raycast(
                    new Vector2(
                            transform.position.x,
                            transform.position.y + height / 2 + 0.01f
                        ),
                    Vector2.up,
                    0.02f
                );

            if((leftHit.collider != null && collision.gameObject == leftHit.collider.gameObject) || 
                (rightHit.collider != null && collision.gameObject == rightHit.collider.gameObject) || 
                (centerHit.collider != null && collision.gameObject == centerHit.collider.gameObject))
            {
                Block block = collision.gameObject.GetComponent<Block>();
                block.onHit(this);
            }
            destroyAudio.Play();
        }
    }

    // Hurt Player
    public void HurtPlayer()
    {
        if(!hasPowerup)
        {
            if(!isInvincible)
            {
                gameObject.GetComponent<Collider2D>().enabled = false;

                playerRigidbody.velocity = new Vector2(
                        0,
                        jumpingSpeed
                    );
                isDead = true;
                controllable = false;
                Destroy(gameObject, 3.0f);

                // PLayer's gameover animation
                playerAnimator.SetBool("isDead", isDead = true);

                // Raise OnLose event
                if(OnLose != null)
                {
                    OnLose();
                }
                hurtAudio.Play();
            }
        }
        else
        {
            hurtAudio.Play();
            hasPowerup = false;
            transform.localScale = Vector3.one;
            StartCoroutine(InvincibilityRoutine(2.0f));
            height /= powerupScale;
        }
    }

    IEnumerator InvincibilityRoutine(float duration)
    {
        isInvincible = true;
        int blinkAmount = 20;
        for(int i = 0; i < blinkAmount; i++)
        {
            container.SetActive(i % 2 == 0);
            yield return new WaitForSeconds(duration / blinkAmount);
        }
        container.SetActive(true);
        isInvincible = false;
    }
}
