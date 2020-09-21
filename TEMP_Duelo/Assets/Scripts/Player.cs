using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Controller Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float gravity = 9.81f;

    [Header("Camera Settings")]
    [SerializeField] private float cameraSensitivity = 1f;

    private CharacterController characterController;
    private Camera mainCamera;

    private Vector3 moveDirection;
    private Vector3 moveVelocity;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;

        if (characterController == null)
        {
            Debug.LogError("The character controller is NULL.");
        }
        if (mainCamera == null)
        {
            Debug.LogError("The main camera is NULL.");
        }

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        HandleCamera();
        HandleMovement();
        HandleCursor();
    }

    private void HandleMovement()
    {
        if (characterController.isGrounded)
        {
            int moveX = 0;
            int moveZ = 0;

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

            moveDirection = new Vector3(moveX, 0, moveZ);
            moveVelocity = moveDirection * moveSpeed * Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                moveVelocity.y = jumpHeight;
            }
        }

        moveVelocity.y -= gravity * Time.deltaTime;

        moveVelocity = transform.TransformDirection(moveVelocity);

        characterController.Move(moveVelocity);
    }

    private void HandleCamera()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Look left and right
        Vector3 currentRotation = transform.localEulerAngles;
        currentRotation.y += mouseX * cameraSensitivity;
        transform.localRotation = Quaternion.AngleAxis(currentRotation.y, Vector3.up);

        // Look up and down
        Vector3 currentCameraRotation = mainCamera.transform.localEulerAngles;
        currentCameraRotation.x -= mouseY * cameraSensitivity;
        mainCamera.transform.localRotation = Quaternion.AngleAxis(currentCameraRotation.x, Vector3.right);
    }

    private void HandleCursor()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
