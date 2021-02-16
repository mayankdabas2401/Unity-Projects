using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 1f;
    public LayerMask enemyMask;
    public Rigidbody bulletRigidbody;
    public float Speed
    {
        set
        {
            speed = value;
        }
    }

    private float speed = 35f;
    private float destroyTime = 1.5f;
    private float width = 0.1f;
    
    // Start is called before the first frame update
    void Start()
    {
        Collider[] colliders = Physics.OverlapSphere(
                transform.position,
                2f,
                enemyMask
            );

        if(colliders.Length > 0)
        {
            OnHit(colliders[0], transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float moveDistance = Time.deltaTime * speed;
        CollosionDetection(moveDistance);

        transform.Translate(Vector3.forward * moveDistance);
        Destroy(gameObject, destroyTime);
    }

    void CollosionDetection(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, moveDistance + width, enemyMask, QueryTriggerInteraction.Collide))
        {
            OnHit(hit.collider, hit.point);
        }
    }

    void OnHit(Collider collider, Vector3 hitPoint)
    {
        IDamageable damageable = collider.gameObject.GetComponent<IDamageable>();

        if (damageable != null) damageable.TakeHit(damage, hitPoint, transform.forward);
        Destroy(gameObject);
    }
}
