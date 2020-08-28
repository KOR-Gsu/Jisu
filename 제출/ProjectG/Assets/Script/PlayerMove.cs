using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 15f;
    public float rotateSpeed = 180f;

    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;
    
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Rotate();
        Move();

        playerAnimator.SetFloat("Move", playerInput.move);
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
}
