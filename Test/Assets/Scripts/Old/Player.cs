using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Controller Settings")]
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = 9.81f;

    [Header("Camera Settings")]
    [SerializeField] private float cameraSensitivity = 1f;

    [Header("CombatSettings")]
    [SerializeField] private int attackDamage;
    [SerializeField] private float hitRadius;
    [SerializeField] private LayerMask enemiesLayer;
    [SerializeField] private float attackTimerMax;

    private CharacterController characterController;
    private Camera mainCamera;
    private HealthSystem healthSystem;
    private Transform attackTransform;
    private Vector3 moveDirection;
    private Vector3 moveVelocity;
    private float attackTimer;
    private bool canAttack;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        healthSystem = GetComponent<HealthSystem>();
        mainCamera = Camera.main;
        attackTransform = transform.Find("attackTransform");

        if (characterController == null)
        {
            Debug.LogError("The Character Controller is NULL.");
        }
        if (mainCamera == null)
        {
            Debug.LogError("The Main Camera is NULL.");
        }
        if (attackTransform == null)
        {
            Debug.LogError("The Attack Transform is NULL.");
        }

        Cursor.lockState = CursorLockMode.Locked;

        healthSystem.OnDamaged += HealthSystem_OnDamaged;
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        Debug.Log("Damaging Player: " + healthSystem.GetHealthAmount() + " hp left.");
    }

    private void Update()
    {
        HandleCamera();
        HandleMovement();
        HandleCursor();
        HandleCombat();
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

        //// Look up and down
        //Vector3 currentCameraRotation = mainCamera.transform.localEulerAngles;
        //currentCameraRotation.x -= mouseY * cameraSensitivity;
        //currentCameraRotation.x = Mathf.Clamp(currentCameraRotation.x, 15, 28);
        //mainCamera.transform.localRotation = Quaternion.AngleAxis(currentCameraRotation.x, Vector3.right);
    }

    private void HandleCursor()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void HandleCombat()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f)
        {
            canAttack = true;
            attackTimer += attackTimerMax;
        }

        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            Debug.Log("Attacking...");
            canAttack = false;

            Collider[] enemiesHitColliderArray = Physics.OverlapSphere(attackTransform.position, hitRadius, enemiesLayer);
            foreach (Collider enemyHitCollider in enemiesHitColliderArray)
            {
                if (enemyHitCollider != null)
                {
                    enemyHitCollider.GetComponent<HealthSystem>().Damage(attackDamage);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackTransform != null)
        {
            Gizmos.DrawWireSphere(attackTransform.position, hitRadius);
        }
    }
}
