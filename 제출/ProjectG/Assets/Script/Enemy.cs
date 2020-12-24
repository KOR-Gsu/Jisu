using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : LivingEntity
{
    public LayerMask TargetLayer;

    private LivingEntity targetEntity;
    private NavMeshAgent pathFinder;
    private Animator enemyAnimator;
    private Canvas hpCanvas;
    private GameObject hpBar;
    private Slider hpSlider;
    private Vector3 hpBarOffset;
    private Text hpText;

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
        health = startingHealth;
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
        hpBarOffset = new Vector3(0, 0, 0);

        StartCoroutine(UpdatePath());
        StartCoroutine(UpdateAttack());
    }
    
    void Update()
    {
        if (isMarking && hpBar == null)
            ShowHPBar();
        if(!isMarking && hpBar != null)
            HideHPBar();

        if (isAttackAble)
        {
            enemyAnimator.SetFloat("Move", 0);
            Attack();
        }
        else
        {
            if (hasTarget)
                enemyAnimator.SetFloat("Move", 1);
            else
                enemyAnimator.SetFloat("Move", 0);
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
                    enemyAnimator.SetInteger("Attack", 1);
                    targetEntity.OnDamage(damage);

                    lastAttackTime = Time.time;
                }
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
                enemyAnimator.SetInteger("Attack", 2);
                enemyAnimator.SetFloat("Move", 0);
                if (pathFinder.enabled)
                {
                    pathFinder.ResetPath();
                    pathFinder.enabled = false;
                }
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

    public override bool OnDamage(float damage)
    {
        if (base.OnDamage(damage))
            return true;

        float curHp = health / startingHealth;
        hpSlider.value = health;
        hpText.text = ((int)(curHp * 100)).ToString() + "%";

        enemyAnimator.SetTrigger("GetHit");

        return false;
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

    private void ShowHPBar()
    {
        if (hpBar != null)
        {
            hpBar.SetActive(true);
            return;
        }

        hpCanvas = GameObject.Find("UI").GetComponent<Canvas>();
        hpBar = Instantiate<GameObject>(healthBarPrefab, hpCanvas.transform);
        hpSlider = hpBar.GetComponentInChildren<Slider>();
        hpText = hpBar.GetComponentInChildren<Text>();

        var _hpBar = hpBar.GetComponent<MonsterHPBar>();
        _hpBar.targetTransform = this.gameObject.transform;
        _hpBar.offSet = new Vector2(0, 50);
    }

    private void HideHPBar()
    {
        hpBar.SetActive(false);
    }
}