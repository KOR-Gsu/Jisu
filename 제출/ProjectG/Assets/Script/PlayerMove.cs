using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 15f;
    public float rotateSpeed = 180f;

    public float damage = 5f;
    public float attackRange;
    public float intvlAttackTime;
    private float lastAttackTime;

    public LayerMask targetLayer;

    private LivingEntity targetEntity;
    private PlayerInput playerInput;
    private PlayerHP playerHP;
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;

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

                targetEntity.OnDamage(damage);

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
