using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private float lastAttackTime;
    private Enemy targetEntity;
    private PlayerInput playerInput;
    private PlayerStat playerStat;
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;

    private bool isInRange
    {
        get
        {
            if (Vector3.Distance(transform.position, targetEntity.transform.position) <= playerStat.attackRange)
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
    
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        playerStat = GetComponent<PlayerStat>();
    }

    void Update()
    {
        if(playerInput.targeting)
            GetClickedObject();

        if (playerInput.attack)
        {
            if (isAttackAble && isInRange)
                Attack();
        }
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
        Vector3 moveDistance = playerStat.moveSpeed * Time.deltaTime * transform.forward * playerInput.move;

        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    }

    void Rotate()
    {
        float rotateAngle = playerStat.rotateSpeed * Time.deltaTime * playerInput.rotate;

        playerRigidbody.rotation *= Quaternion.Euler(0, rotateAngle, 0);
    }

    void Attack()
    {
        if (Time.time >= lastAttackTime + playerStat.intvlAttackTime)
        {
            transform.LookAt(targetEntity.transform);
            playerAnimator.SetInteger("Attack", 1);
            targetEntity.OnDamage(playerStat.attackDamage);

            if (targetEntity.dead)
                playerStat.GetExp(50);

            lastAttackTime = Time.time;
        }
        else
            playerAnimator.SetInteger("Attack", 2);
    }

    private void GetClickedObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray.origin, ray.direction * 10, out RaycastHit hit))
        {
            GameObject target = hit.collider.gameObject;

            if (target.GetComponent<Shop>() != null)
            {
                target.GetComponent<Shop>().OpenShop();
            }

            if (target.GetComponent<Enemy>() != null)
            {
                targetEntity = hit.collider.gameObject.GetComponent<Enemy>();
                targetEntity.Marking(Color.red);
            }
        }
    }
}