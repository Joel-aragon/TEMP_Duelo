using UnityEngine;

public class PlayerMovementSystem : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float turnSmoothTime = 0.1f;

    private CharacterController characterController;
    private Camera mainCamera;
    private float turnSmoothVelocity;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
    }

    public void HandleMovement(Vector3 moveDirection)
    {
        TurnTowardsMoveDirectionSmoothly(moveDirection, out float targetAngle);

        moveDirection = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;
        Vector3 moveVelocity = moveDirection * moveSpeed * Time.deltaTime;
        characterController.Move(moveVelocity);
    }

    private void TurnTowardsMoveDirectionSmoothly(Vector3 moveDirection, out float targetAngle)
    {
        targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
        float smoothedTargetAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, smoothedTargetAngle, 0f);
    }
}