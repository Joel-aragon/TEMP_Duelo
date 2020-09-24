using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovementSystem playerMovementSystem;

    private void Awake()
    {
        playerMovementSystem = GetComponent<PlayerMovementSystem>();
    }

    private void Update()
    {
        HandleInputMovement();
    }

    private void HandleInputMovement()
    {
        float moveZ = 0f;
        float moveX = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            moveZ = +1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveZ = -1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveX = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveX = +1;
        }

        Vector3 moveDirection = new Vector3(moveX, 0f, moveZ).normalized;

        bool isMoving = moveDirection.magnitude >= Mathf.Epsilon;

        if (isMoving)
        {
            playerMovementSystem.HandleMovement(moveDirection);
        }
    }
}
