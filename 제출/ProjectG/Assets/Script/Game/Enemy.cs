using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : LivingEntity
{
    public LayerMask targetLayer;

    private LivingEntity targetEntity;
    private NavMeshAgent pathFinder;
    private Animator enemyAnimator;
    private GameObject hpGauge;
    private MonsterHPGauge hpGaugeScript;

    public GameObject hpGaugePrefab;
    public float damage = 3f;
    public float intvlAttackTime;
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
        maxHP = newHealth;
        currentHP = maxHP;
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
        damagedTextColor = Color.white;

        StartCoroutine(UpdatePath());
        StartCoroutine(UpdateAttack());
    }
    
    void Update()
    {
        if (isMarking && hpGauge == null)
            ShowHPBar();

        if (isAttackAble && hasTarget)
            Attack();
    }

    private void Attack()
    {
        if (!dead) 
        {
            if (Time.time >= lastAttackTime + intvlAttackTime)
            {
                enemyAnimator.SetInteger("Attack", 1);
                targetEntity.OnDamage(damage);

                if (targetEntity.dead)
                    GameManager.instance.EndGame();

                lastAttackTime = Time.time;
            }
            else
                enemyAnimator.SetInteger("Attack", 2);
        }
    }

    private IEnumerator UpdateAttack()
    {
        while (!dead)
        {
            bool check = false;
            Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange, targetLayer);

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
                enemyAnimator.SetInteger("Attack", 2);
                enemyAnimator.SetFloat("Move", 0);
            }
            else
            {
                isAttackAble = false;
                enemyAnimator.SetInteger("Attack", 0);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator UpdatePath()
    {
        while (!dead)
        {
            if (hasTarget && !isAttackAble)
            {
                enemyAnimator.SetFloat("Move", 1);
                pathFinder.enabled = true;
                pathFinder.SetDestination(targetEntity.transform.position);
            }
            else
            {
                pathFinder.enabled = false;
                Collider[] colliders = Physics.OverlapSphere(transform.position, 30f, targetLayer);

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

    public override void OnDamage(float damage)
    {
        base.OnDamage(damage);

        hpGauge.GetComponent<MonsterHPGauge>().Initialize(currentHP / maxHP);
    }

    public override void Die()
    {
        base.Die();

        Collider[] enemyColliders = GetComponents<Collider>();
        for (int i = 0; i < enemyColliders.Length; i++)
            enemyColliders[i].enabled = false;
        
        pathFinder.enabled = false;

        if(hpGauge != null)
            Destroy(hpGauge);

        enemyAnimator.SetTrigger("Die");
    }

    private void ShowHPBar()
    {
        if (hpGauge != null)
        {
            hpGauge.gameObject.SetActive(true);
            return;
        }

        hpGauge = Instantiate<GameObject>(hpGaugePrefab, UIManager.instance.myCanvas.transform);
        hpGaugeScript = hpGauge.GetComponentInChildren<MonsterHPGauge>();
        hpGaugeScript.targetTransform = hudPos;
        hpGaugeScript.Initialize(currentHP / maxHP);
    }

    private void HideHPBar()
    {
        hpGauge.gameObject.SetActive(false);
    }
}