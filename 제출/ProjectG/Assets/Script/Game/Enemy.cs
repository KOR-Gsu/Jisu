using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : LivingEntity
{
    private LivingEntity targetEntity;
    private NavMeshAgent pathFinder;
    private Animator enemyAnimator;
    private GameObject hpGauge;
    private MonsterHPGauge hpGaugeScript;

    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private string hpGaugePrefabPath;

    private float attackDamage;
    private float defense;
    private float attackRange;
    private float intvlAttackTime;
    private float lastAttackTime;
    public float exp { get; private set; }
    public float gold { get; private set; }

    private Define.EnemyState _enemyState;
    public Define.EnemyState enemyState
    {
        get { return _enemyState; }
        set
        {
            _enemyState = value;
            switch (_enemyState)
            {
                case Define.EnemyState.Die:
                    enemyAnimator.CrossFade("Die", 0.1f);
                    break;
                case Define.EnemyState.Idle:
                    enemyAnimator.CrossFade("Idle", 0.1f);
                    break;
                case Define.EnemyState.Moving:
                    enemyAnimator.CrossFade("Move", 0.1f);
                    break;
                case Define.EnemyState.Attack:
                    int attack = Random.Range(1, 2);
                    enemyAnimator.CrossFade("Attack" + attack.ToString(), 0.1f, -1, 0f);
                    break;
            }
        }
    }
    private bool isInRange
    {
        get
        {
            if (Vector3.Distance(transform.position, targetEntity.transform.position) <= attackRange)
                return true;

            return false;
        }
    }
    private bool isAttackAble
    {
        get
        {
            if (targetEntity != null && !targetEntity.dead)
                return true;

            return false;
        }
    }

    public void Initializing(MonsterData data)
    {
        maxHP = data.maxHP;
        currentHP = maxHP;
        attackDamage = data.attackDamage;
        defense = data.defense;
        attackRange = data.attackRange;
        pathFinder.speed = data.moveSpeed;
        pathFinder.angularSpeed = data.rotateSpeed;
        intvlAttackTime = data.intvlAttackTime;
        exp = data.exp;
        gold = data.gold;
    }

    void Awake()
    {
        pathFinder = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
    }
    
    void Start()
    {
        lastAttackTime = 0;
        damagedTextColor = Color.white;

        StartCoroutine(UpdateMovingPath());
        StartCoroutine(UpdateAttackTarget());
    }

    protected override void OnEnable()
    {
        base.OnEnable(); 
        
        Collider[] enemyColliders = GetComponents<Collider>();
        for (int i = 0; i < enemyColliders.Length; i++)
            enemyColliders[i].enabled = true;
    }

    void Update()
    {
        if (!dead)
        {
            if (isMarking && hpGauge == null)
                ShowHPBar();

            switch (enemyState)
            {
                case Define.EnemyState.Idle:
                    UpdateIdle();
                    break;
                case Define.EnemyState.Moving:
                    UpdateMoving();
                    break;
                case Define.EnemyState.Attack:
                    if (isInRange && isAttackAble)
                        UpdateAttack();
                    break;
            }
        }
    }

    private void UpdateIdle()
    {
        if (dead)
            enemyState = Define.EnemyState.Die;
    }

    private void UpdateMoving()
    {
        Vector3 dir = targetEntity.transform.position - transform.position;
        if (dir.magnitude < 0.0001f)
        {
            enemyState = Define.EnemyState.Idle;
        }
        else
        {
            pathFinder.SetDestination(targetEntity.transform.position);
        }
    }

    private void UpdateAttack()
    {
        if (!dead) 
        {
            if (Time.time >= lastAttackTime + intvlAttackTime)
            {
                enemyState = Define.EnemyState.Attack;
                lastAttackTime = Time.time;
            }
            else
                enemyState = Define.EnemyState.Idle;
        }
    }

    public void OnAttackEvent()
    {
        transform.LookAt(targetEntity.transform);
        targetEntity.OnDamage(attackDamage);

        if (targetEntity.dead)
            GameManager.instance.EndGame();
    }

    private IEnumerator UpdateAttackTarget()
    {
        while (!dead)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange, targetLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();

                if (livingEntity != null && !livingEntity.dead)
                {
                    targetEntity = livingEntity;
                }                
            }
            yield return new WaitForSeconds(0.25f);
        }
    }

    private IEnumerator UpdateMovingPath()
    {
        while (!dead)
        {
            pathFinder.enabled = false;
            Collider[] colliders = Physics.OverlapSphere(transform.position, 20f, targetLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();

                if (livingEntity != null && !livingEntity.dead)
                {
                    targetEntity = livingEntity;
                    pathFinder.enabled = true;
                    break;
                }
            }
            yield return new WaitForSeconds(0.25f);
        }
    }

    public override void OnDamage(float damage)
    {
        float finalDamage = damage - defense;
        if (finalDamage <= 0)
            finalDamage = 0;

        base.OnDamage(finalDamage);

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

        Invoke("SetActiveFalse", 4f);
    }

    private void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }

    private void ShowHPBar()
    {
        if (hpGauge != null)
        {
            hpGauge.gameObject.SetActive(true);
            return;
        }

        hpGauge = ResourceManager.instance.Instantiate(hpGaugePrefabPath, UIManager.instance.myCanvas.transform);
        hpGaugeScript = hpGauge.GetComponentInChildren<MonsterHPGauge>();
        hpGaugeScript.targetTransform = hudPos;
        hpGaugeScript.Initialize(currentHP / maxHP);
    }
}