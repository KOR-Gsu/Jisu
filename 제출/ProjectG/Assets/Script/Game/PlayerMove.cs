using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviour
{
    private Vector3 destPos = Vector3.zero;
    private float lastAttackTime = 0;

    private Enemy targetEntity;
    private PlayerInput playerInput;
    private PlayerStat playerStat;
    private Animator playerAnimator;

    [SerializeField] private GameObject destPosImage;

    private Define.PlayerState _playerState;
    public Define.PlayerState playerState
    {
        get { return _playerState; }
        set
        {
            _playerState = value;
            switch (_playerState)
            {
                case Define.PlayerState.Die:
                    playerAnimator.CrossFade("Die", 0.1f);
                    break;
                case Define.PlayerState.Idle:
                    playerAnimator.CrossFade("Idle", 0.1f);
                    break;
                case Define.PlayerState.Moving:
                    playerAnimator.CrossFade("Move", 0.1f);
                    break;
                case Define.PlayerState.Attack:
                    int attack = Random.Range(1, 2);
                    playerAnimator.CrossFade("Attack" + attack.ToString(), 0.1f, -1, 0f);
                    break;
                case Define.PlayerState.Skill:
                    break;
            }
        }
    }

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
        playerStat = GetComponent<PlayerStat>();
        playerAnimator = GetComponent<Animator>();

        InputManager.instance.keyAction -= OnSkillKeyPress;
        InputManager.instance.keyAction += OnSkillKeyPress;
        InputManager.instance.mouseAction -= OnMouse0Clicked;
        InputManager.instance.mouseAction += OnMouse0Clicked;
    }

    private void Update()
    {
        if (!playerStat.dead)
        {
            switch (playerState)
            {
                case Define.PlayerState.Idle:
                    UpdateIdle();
                    break;
                case Define.PlayerState.Moving:
                    UpdateMoving();
                    break;
                case Define.PlayerState.Attack:
                    if (isInRange && isAttackAble)
                        UpdateAttack();
                    break;
            }
        }
    }

    private void UpdateIdle()
    {
        destPosImage.SetActive(false);

        if (playerStat.dead)
            playerState = Define.PlayerState.Die;
    }

    private void UpdateMoving()
    {
        destPosImage.SetActive(true);

        Vector3 dir = destPos - transform.position;
        if (dir.magnitude < 0.0001f)
        {
            playerState = Define.PlayerState.Idle;
        }
        else
        {
            if (Physics.Raycast(transform.position, dir, 1.5f, LayerMask.GetMask("Terrain")))
            {
                playerState = Define.PlayerState.Idle;
                return;
            }

            float moveDist = Mathf.Clamp(playerStat.moveSpeed * Time.deltaTime, 0, dir.magnitude);
            transform.position = transform.position + dir.normalized * moveDist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), playerStat.rotateSpeed * Time.deltaTime);
            transform.LookAt(destPos);
        }
    }

    private void UpdateAttack()
    {
        if (Time.time >= lastAttackTime + playerStat.intvlAttackTime)
        {
            playerState = Define.PlayerState.Attack;
            lastAttackTime = Time.time;
        }
        else
            playerState = Define.PlayerState.Idle;
    }

    public void OnAttackEvent()
    {
        transform.LookAt(targetEntity.transform);
        targetEntity.OnDamage(playerStat.attackDamage);

        if (targetEntity.dead)
        {
            playerStat.GetExp((int)targetEntity.exp, (int)targetEntity.gold);
        }
    }

    private void OnSkillKeyPress()
    {
        if (!playerStat.dead)
        {
            if (playerInput.skill1)
            {
                playerState = Define.PlayerState.Skill;
            }
            if (playerInput.skill2)
            {
                playerState = Define.PlayerState.Skill;
            }
        }
    }

    private void OnMouse0Clicked(Define.Mouse mouse, Define.MouseEvent evt)
    {
        if (!playerStat.dead)
        {
            if (mouse != Define.Mouse.Mouse_0 || evt != Define.MouseEvent.Click)
                return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitEnemy, 100f, LayerMask.GetMask("Enemy")))
            {
                if (hitEnemy.collider.gameObject.GetComponent<Enemy>() != null)
                {
                    targetEntity = hitEnemy.collider.gameObject.GetComponent<Enemy>();
                    targetEntity.Marking(Color.red);
                }
            }
            else if (Physics.Raycast(ray, out RaycastHit hitShop, 100f, LayerMask.GetMask("Shop")))
            {
                if (hitShop.collider.gameObject.GetComponent<Shop>() != null)
                {
                    hitShop.collider.gameObject.GetComponent<Shop>().OpenShop();
                }
            }
            else if (Physics.Raycast(ray, out RaycastHit hit, 100f, LayerMask.GetMask("Terrain")))
                return;
            else if (Physics.Raycast(ray, out RaycastHit hitGround, 100f, LayerMask.GetMask("Ground")))
            {
                destPos = hitGround.point;
                destPosImage.transform.position = new Vector3(destPos.x, destPos.y + 0.5f, destPos.z);

                playerState = Define.PlayerState.Moving;
            }
        }
    }
}