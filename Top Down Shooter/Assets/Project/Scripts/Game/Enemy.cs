using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : LivingEntity
{
    public enum EnemyState
    {
        Idle,
        Chase,
        Attack
    }

    public ParticleSystem deathEffect;
    public static event System.Action OnDeathStatic;

    private float minAttackDistance = 1.5f;
    private float attackRate = 1.0f;
    private float nextAttackTime;
    private float enemyColliderRadius;
    private float targetColliderRadius;
    private float damage = 1f;

    private bool hasTarget;

    private Transform target;
    private NavMeshAgent pathFinder;
    private LivingEntity targetEntity;
    private Material skinMaterial;
    private Color originalColor;

    private EnemyState currentState;

    private void Awake()
    {
        // Path finder
        pathFinder = GetComponent<NavMeshAgent>();
        
        if (FindObjectOfType<Player>() != null)
        {
            // Target
            target = GameObject.FindGameObjectWithTag("Player").transform;
            targetEntity = target.gameObject.GetComponent<LivingEntity>();

            // Has target
            hasTarget = true;

            // Collider radius
            enemyColliderRadius = gameObject.GetComponent<CapsuleCollider>().radius;
            targetColliderRadius = target.gameObject.GetComponent<CapsuleCollider>().radius;
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        if (hasTarget)
        {
            // Path finder
            currentState = EnemyState.Chase;
            StartCoroutine("PathFinderRoutine");

            // On target death
            targetEntity.OnDeath += OnTargetDeath;
        }
    }

    void OnTargetDeath()
    {
        hasTarget = false;
        currentState = EnemyState.Idle;
    }

    public void SetCharacterstics(int hitsToKill, float enemyHealth, float speed, Color skinColor)
    {
        // Enemy Speed
        pathFinder.speed = speed;

        // Set damage
        if(hasTarget)
        {
            damage = Mathf.Ceil(targetEntity.startingHealth / hitsToKill);
        }

        // Materials
        deathEffect.startColor = new Color(
                skinColor.r,
                skinColor.g,
                skinColor.b,
                1
            );
        skinMaterial = gameObject.GetComponent<Renderer>().material;
        skinMaterial.color = skinColor;
        originalColor = skinMaterial.color;
    }

    // Update is called once per frame
    void Update()
    {
        if(hasTarget)
        {
            if (Time.time > nextAttackTime)
            {
                float sqrDistance = (target.position - transform.position).sqrMagnitude;

                if (sqrDistance < Mathf.Pow(minAttackDistance + enemyColliderRadius + targetColliderRadius, 2))
                {
                    nextAttackTime = Time.time + attackRate;
                    AudioManager.instance.PlaySound("Enemy Attack", transform.position);
                    StartCoroutine("AttackRoutine");
                }
            }
        }
    }

    IEnumerator AttackRoutine()
    {
        currentState = EnemyState.Attack;
        pathFinder.enabled = false;

        Vector3 originalPosition = transform.position;
        Vector3 direction = (target.position - transform.position).normalized;
        Vector3 attackPosition = target.position - direction * enemyColliderRadius;

        float attackSpeed = 3f;
        float attackPercent = 0f;

        skinMaterial.color = Color.red;

        bool damageApplied = false;

        while(attackPercent <= 1f)
        {
            if(attackPercent >= 0.5f && !damageApplied)
            {
                damageApplied = true;
                targetEntity.TakeDamage(damage);
            }

            attackPercent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(attackPercent, 2) + attackPercent) * 4;

            transform.position = Vector3.Lerp(
                    originalPosition,
                    attackPosition,
                    interpolation
                );

            yield return null;
        }

        skinMaterial.color = originalColor;

        currentState = EnemyState.Chase;
        pathFinder.enabled = true;
    }

    IEnumerator PathFinderRoutine()
    {
        float refreshRate = 0.25f;

        while(hasTarget)
        {
            if(currentState == EnemyState.Chase)
            {
                Vector3 direction = (target.position - transform.position).normalized;

                Vector3 targetPosition = target.position - direction * (enemyColliderRadius + targetColliderRadius + minAttackDistance / 2);
                if (!isDead) pathFinder.SetDestination(targetPosition);
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }

    public override void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        base.TakeHit(damage, hitPoint, hitDirection);

        AudioManager.instance.PlaySound("Impact", transform.position);

        if (damage >= health)
        {
            if(OnDeathStatic != null)
            {
                OnDeathStatic();
            }

            AudioManager.instance.PlaySound("Enemy Death", transform.position);
            Destroy(
                    Instantiate(
                            deathEffect.gameObject,
                            hitPoint,
                            Quaternion.FromToRotation(Vector3.forward, hitDirection)
                        ) as GameObject,
                    deathEffect.startLifetime
                );
        }
    }
}
