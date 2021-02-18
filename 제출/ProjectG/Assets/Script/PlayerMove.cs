using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [HideInInspector] public float moveSpeed;
    [HideInInspector] public float rotateSpeed;
    [HideInInspector] public float attackDamage;
    [HideInInspector] public float attackRange;
    [HideInInspector] public float intvlAttackTime;

    private float lastAttackTime;
    private LivingEntity targetEntity;
    private PlayerInput playerInput;
    private PlayerHP playerHP;
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;

    public LayerMask targetLayer;

    private bool isAttackAble
    {
        get
        {
            if (targetEntity != null && !targetEntity.dead)
                return true;

            return false;
        }
    }
    
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        playerHP = GetComponent<PlayerHP>();

        StartCoroutine(UpdateAttackTarget());
    }

    public void initializing(PlayerData data)
    {
        data.dataDictionary.TryGetValue("moveSpeed", out moveSpeed);
        data.dataDictionary.TryGetValue("rotateSpeed", out rotateSpeed);

        data.dataDictionary.TryGetValue("attackDamage", out attackDamage);
        data.dataDictionary.TryGetValue("attackRange", out attackRange);
        data.dataDictionary.TryGetValue("intvlAttackTime", out intvlAttackTime);
    }

    void Update()
    {
        if (playerInput.attack)
            Attack();
        else
            playerAnimator.SetInteger("Attack", 0);
    }

    void FixedUpdate()
    {
        if (playerAnimator.GetInteger("Attack") == 0)
        {
            Rotate();
            Move();
            
            playerAnimator.SetFloat("Move", playerInput.move);
        }
    }

    void Move()
    {
        Vector3 moveDistance = moveSpeed * Time.deltaTime * transform.forward * playerInput.move;

        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    }

    void Rotate()
    {
        float rotateAngle = rotateSpeed * Time.deltaTime * playerInput.rotate;

        playerRigidbody.rotation *= Quaternion.Euler(0, rotateAngle, 0);
    }

    void Attack()
    {
        if (Time.time >= lastAttackTime + intvlAttackTime)
        {
            if (isAttackAble)
            {
                transform.LookAt(targetEntity.transform);

                targetEntity.OnDamage(attackDamage);

                if(targetEntity.dead)
                    playerHP.GetExp(50);
            }

            playerAnimator.SetInteger("Attack", 1);
            lastAttackTime = Time.time;
        }
        else
            playerAnimator.SetInteger("Attack", 2);
    }

    private IEnumerator UpdateAttackTarget()
    {
        while (true)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange, targetLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();

                if (livingEntity.isMarking)
                    break;

                if (livingEntity != null && !livingEntity.dead)
                {
                    if (targetEntity != null)
                        targetEntity.UnMarking();

                    targetEntity = livingEntity;
                    targetEntity.Marking(Color.red);
                    break;
                }
            }
            yield return new WaitForSeconds(0.25f);
        }
    }
}
