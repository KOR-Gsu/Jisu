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
    public float timeBetAttack;
    public float attackRange;
    private float lastAttackTime;

    public bool isAttackAble;
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
            if (hasTarget)
                enemyAnimator.SetFloat("Move", 1);
            else
                enemyAnimator.SetFloat("Move", 0);
        }
        else
        {
            enemyAnimator.SetFloat("Move", 0);
            Attack();
        }
    }

    private void Attack()
    {
        if (!dead) 
        {
            if (Time.time >= lastAttackTime + timeBetAttack)
            {
                if (targetEntity != null && !targetEntity.dead)
                {
                    enemyAnimator.SetFloat("Attack", 1);
                    targetEntity.OnDamage(damage);

                    lastAttackTime = Time.time;
                }
            }
        }
        
    }

    private IEnumerator UpdateAttack()
    {
        while (!dead)
        {
            bool check = false;
            Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange, TargetLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();

                if (livingEntity != null && !livingEntity.dead)
                {
                    check = true;
                    break;
                }                
            }

            if(check)
            {
                isAttackAble = true;
                if (pathFinder.enabled)
                {
                    pathFinder.ResetPath();
                    pathFinder.enabled = false;
                }
            }
            else
            {
                isAttackAble = false;
                enemyAnimator.SetFloat("Attack", 0);
            }

            yield return new WaitForSeconds(0.1f);
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
                    pathFinder.enabled = true;
                    pathFinder.SetDestination(targetEntity.transform.position);
                    enemyAnimator.SetFloat("Move", 1);
                }
                else
                {
                    pathFinder.enabled = false;
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

        enemyAnimator.SetTrigger("Damaged");
    }

    public override void Die()
    {
        base.Die();

        Collider[] enemyColliders = GetComponents<Collider>();
        for (int i = 0; i < enemyColliders.Length; i++)
            enemyColliders[i].enabled = false;
        
        pathFinder.enabled = false;

        enemyAnimator.SetTrigger("Die");
    }
}
