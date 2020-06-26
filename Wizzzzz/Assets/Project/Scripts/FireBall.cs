using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [Header("Movements")]
    public float speed;

    private Rigidbody2D fireballRigidbody;
    // Start is called before the first frame update
    void Start()
    {
        fireballRigidbody = gameObject.GetComponent<Rigidbody2D>();
        fireballRigidbody.velocity = transform.right * speed;
    }
}
