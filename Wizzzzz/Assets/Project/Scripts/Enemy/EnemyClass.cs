using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClass : MonoBehaviour
{
    private bool isDead;
    [SerializeField]
    private GameObject explosionPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "FireBall")
        {
            Destroy(collision.gameObject);
            isDead = true;
        }
    }

    private void Update()
    {   
        if(isDead)
        {
            GameObject explosionInstant = GameObject.Instantiate(explosionPrefab);
            explosionInstant.transform.position = gameObject.transform.position;
            explosionInstant.transform.rotation = gameObject.transform.rotation;
            Destroy(gameObject);
        }
    }
}
