using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    public Rigidbody shellRigidbody;

    public float minimumForce;
    public float maximumForce;
    public float lifeTime = 4f;
    // Start is called before the first frame update
    void Start()
    {
        float force = Random.Range(minimumForce, maximumForce);
        shellRigidbody.AddForce(transform.right * force);
        shellRigidbody.AddTorque(Random.insideUnitSphere * force);

        Destroy(gameObject, lifeTime);
    }
}
