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

    public float damage = 3f;
    public float timeBetAttack = 2f;
    public float attackRange = 0.5f;
    public float lastAttackTime;

    private bool isAttackAble;
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

        isAttackAble = false;
    }
    
    void Start()
    {
        StartCoroutine(UpdatePath());
        StartCoroutine(UpdateAttack());
    }
    
    void Update()
    {
        if (!isAttackAble)
        {
            if(hasTarget)
                enemyAnimator.SetFloat("HasTarget", 1);
            else
                enemyAnimator.SetFloat("HasTarget", 0);
        }
    }

    private IEnumerator UpdateAttack()
    {
        while (!dead)
        {
            isAttackAble = false;
            pathFinder.isStopped = true;

            Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange, TargetLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();

                if (livingEntity != null && !livingEntity.dead)
                {
                    isAttackAble = true;
                    pathFinder.isStopped = true;
                    break;
                }
            }
            
            if (isAttackAble)
            {
                if (targetEntity != null && !targetEntity.dead)
                {
                    targetEntity.OnDamage(damage);
                    enemyAnimator.SetFloat("Attack", (float)Random.Range(1, 2));
                }
            }
            yield return new WaitForSeconds(timeBetAttack);
        }
    }

    private IEnumerator UpdatePath()
    {
        if (!isAttackAble)
        {
            while (!dead)
            {
                if (hasTarget)
                {
                    pathFinder.isStopped = false;
                    pathFinder.SetDestination(targetEntity.transform.position);
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

    public override void OnDamage(float damage)
    {
        base.OnDamage(damage);
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

    //private void OnTriggerStay(Collider other)
    //{
    //    if (!dead)
    //    {
    //        if (Time.time >= lastAttackTime + timeBetAttack)
    //        {
    //            LivingEntity attackTarget = other.GetComponent<LivingEntity>();

    //            if (attackTarget != null && attackTarget == targetEntity)
    //            {
    //                isAttackAble = true;
    //                pathFinder.isStopped = true;

    //                lastAttackTime = Time.time;

    //                Vector3 hitPoint = other.ClosestPoint(transform.position);

    //                attackTarget.OnDamage(damage);
    //                enemyAnimator.SetFloat("Attack", (float)Random.Range(0, 1));
    //            }
    //        }
    //        else
    //            enemyAnimator.SetFloat("Attack", 2f);
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (!dead && isAttackAble)
    //    {
    //        isAttackAble = false;
    //        pathFinder.isStopped = false;
    //        enemyAnimator.SetFloat("hasTarget", 0);
    //    }
    //}
}
