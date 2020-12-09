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
    public float timeBetAttack = 0.5f;
    public float lastAttackTime;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpdatePath());
    }

    // Update is called once per frame
    void Update()
    {
        enemyAnimator.SetBool("hasTarget", hasTarget);
    }

    private IEnumerator UpdatePath()
    {
        while(!dead)
        {
            if(hasTarget)
            {
                pathFinder.isStopped = false;
                pathFinder.SetDestination(targetEntity.transform.position);
            }
            else
            {
                pathFinder.isStopped = true;
                Collider[] colliders = Physics.OverlapSphere(transform.position, 20f, TargetLayer);

                for(int i =0;i<colliders.Length;i++)
                {
                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();

                    if(livingEntity != null && !livingEntity.dead)
                    {
                        targetEntity = livingEntity;
                        break;
                    }

                }
            }

            yield return new WaitForSeconds(0.25f);
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
        if(!dead && Time.time >= lastAttackTime + timeBetAttack)
        {
            LivingEntity attackTarget = other.GetComponent<LivingEntity>();

            if(attackTarget != null && attackTarget == targetEntity)
            {
                lastAttackTime = Time.time;

                Vector3 hitPoint = other.ClosestPoint(transform.position);

                attackTarget.OnDamage(damage, hitPoint);
            }
        }
    }
}
