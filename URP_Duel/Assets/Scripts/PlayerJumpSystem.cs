using UnityEngine;

public class PlayerJumpSystem : MonoBehaviour
{
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float groundCheckRadius = 0.4f;
    [SerializeField] private LayerMask groundMask;

    private CharacterController characterController;
    private Transform groundCheck;

    private Vector3 jumpVelocity;
    private Vector3 jumpAcceleration;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        groundCheck = transform.Find("groundCheck");
    }

    private void Update()
    {
        HandleFall();
        HandleLand();
    }

    public void HandleJump()
    {
        jumpVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    public bool IsGrounded()
    {
        bool isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);
        return isGrounded;
    }

    private void HandleFall()
    {
        jumpVelocity.y += gravity * Time.deltaTime;
        jumpAcceleration.y = jumpVelocity.y * Time.deltaTime;
        characterController.Move(jumpAcceleration);
    }

    private void HandleLand()
    {
        if (IsGrounded() && jumpVelocity.y < 0f)
        {
            jumpVelocity.y = -2f;
        }
    }
}