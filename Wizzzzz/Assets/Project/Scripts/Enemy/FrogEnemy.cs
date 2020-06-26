using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogEnemy : MonoBehaviour
{
    private Rigidbody2D frogRigidbody;
    private Animator frogAnimator;
    // Start is called before the first frame update
    void Start()
    {
        frogRigidbody = gameObject.GetComponent<Rigidbody2D>();
        frogAnimator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        frogAnimator.SetBool("Jump", Mathf.Abs(frogRigidbody.velocity.x) > 0.0f);
    }
}
