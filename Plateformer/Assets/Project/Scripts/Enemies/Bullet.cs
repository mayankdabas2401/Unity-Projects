using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Movements")]
    public float horizontalSpeed;
    public float verticalSpeed;
    [Header("Referenced Components")]
    public GameObject player;

    private Rigidbody2D bulletRigidbody;
    

    // Start is called before the first frame update
    void Start()
    {
        bulletRigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        bulletRigidbody.AddForce(new Vector2(horizontalSpeed, verticalSpeed));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy" 
            && 
            collision.gameObject.GetComponent<Enemy>().shootable)
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.OnHit(player.gameObject);
        }
    }
}
