using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public event System.Action OnDeath;
    public float startingHealth;

    public float health { get; protected set; }
    protected bool isDead;

    protected virtual void Start()
    {
        health = startingHealth;
        isDead = false;
    }

    public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        TakeDamage(damage);
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0f && !isDead) Die();
    }

    public virtual void Die()
    {
        isDead = true;
        if (OnDeath != null) OnDeath();
        Destroy(gameObject);
    }
}
