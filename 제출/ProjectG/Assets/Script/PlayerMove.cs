using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 15f;
    public float rotateSpeed = 180f;

    public float damage = 5f;
    public float attackRange;
    public float timeBetAttack;
    private float lastAttackTime;

    public LayerMask targetLayer;

    private LivingEntity targetEntity;
    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;

    private bool isAttackAble
    {
        get
        {
            if (playerInput.attack > 0)
                return true;

            return false;
        }
    }

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();

        StartCoroutine(UpdateAttackTarget());
    }

    void Update()
    {
        if (isAttackAble)
            Attack();
        else
            playerAnimator.SetFloat("Attack", 0);
    }

    void FixedUpdate()
    {
        if (!isAttackAble)
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
        if (isAttackAble)
        {
            if (Time.time >= lastAttackTime + timeBetAttack)
            {
                if (targetEntity != null && !targetEntity.dead)
                {
                    targetEntity.OnDamage(damage);
                    playerAnimator.SetFloat("Attack", 1);

                    lastAttackTime = Time.time;
                }
            }
        }
    }

    private IEnumerator UpdateAttackTarget()
    {
        while (true)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange, targetLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();

                if (targetEntity == livingEntity)
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
