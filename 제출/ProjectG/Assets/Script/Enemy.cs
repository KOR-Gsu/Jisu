using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : LivingEntity
{
    public LayerMask TargetLayer;

    private LivingEntity targetEntity;
    private NavMeshAgent pathFinder;

    private Animator enemyAnimator;

    public float damage = 5f;
    public float timeBetAttack = 0.8f;
    public float lastAttackTime;

    private bool isAttack;
    private bool hasTarget
    {
        get
        {
            if (targetEntity != null && !targetEntity.dead)
                return true;

            return false;
        }
    }

    public void Setup(float newHealth, float newDamage, float newSpeed)
    {
        startingHealth = newHealth;
        damage = newDamage;
        pathFinder.speed = newSpeed;
    }

    void Awake()
    {
        pathFinder = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();

        isAttack = false;
    }
    
    void Start()
    {
        StartCoroutine(UpdatePath());
    }
    
    void Update()
    {
        if(!hasTarget && !isAttack)
            enemyAnimator.SetFloat("hasTarget", 0);
    }

    private IEnumerator UpdatePath()
    {
        if (!isAttack)
        {
            while (!dead)
            {
                if (hasTarget)
                {
                    pathFinder.isStopped = false;
                    pathFinder.SetDestination(targetEntity.transform.position);
                    enemyAnimator.SetFloat("hasTarget", 1);
                }
                else
                {
                    pathFinder.isStopped = true;
                    Collider[] colliders = Physics.OverlapSphere(transform.position, 30f, TargetLayer);

                    for (int i = 0; i < colliders.Length; i++)
                    {
                        LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();

                        if (livingEntity != null && !livingEntity.dead)
                        {
                            targetEntity = livingEntity;
                            break;
                        }

                    }
                }
                yield return new WaitForSeconds(0.25f);
            }
        }
    }

    public override void OnDamage(float damage, Vector3 hitPoint)
    {
        base.OnDamage(damage, hitPoint);
    }

    public override void Die()
    {
        base.Die();

        Collider[] enemyColliders = GetComponents<Collider>();
        for (int i = 0; i < enemyColliders.Length; i++)
            enemyColliders[i].enabled = false;

        pathFinder.isStopped = true;
        pathFinder.enabled = false;

        enemyAnimator.SetTrigger("Die");
    }

    private void OnTriggerStay(Collider other)
    {
        if (!dead)
        {
            if (Time.time >= lastAttackTime + timeBetAttack)
            {
                LivingEntity attackTarget = other.GetComponent<LivingEntity>();

                if (attackTarget != null && attackTarget == targetEntity)
                {
                    isAttack = true;
                    pathFinder.isStopped = true;

                    lastAttackTime = Time.time;

                    Vector3 hitPoint = other.ClosestPoint(transform.position);

                    attackTarget.OnDamage(damage, hitPoint);
                    enemyAnimator.SetFloat("Attack", (float)Random.Range(0, 1));
                }
            }
            else
                enemyAnimator.SetFloat("Attack", 2f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!dead && isAttack)
        {
            isAttack = false;
            pathFinder.isStopped = false;
            enemyAnimator.SetFloat("hasTarget", 0);
        }
    }
}
