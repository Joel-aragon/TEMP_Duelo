using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum State
    {
        Alive,
        Dead
    }

    [SerializeField] private float timerAttackMax = 1f;
    private float timerAttack;
    private bool canAttack = true;

    private PlayerMovementSystem playerMovementSystem;
    private PlayerJumpSystem playerJumpSystem;
    private MeleeAttackSystem meleeAttackSystem;
    private State state;

    private void Awake()
    {
        playerMovementSystem = GetComponent<PlayerMovementSystem>();
        playerJumpSystem = GetComponent<PlayerJumpSystem>();
        meleeAttackSystem = GetComponent<MeleeAttackSystem>();

        state = State.Alive;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Alive:
                HandleInputMovement();
                HandleInputJump();
                HandleInputAttack();
                break;
            case State.Dead:
                break;
        }
    }

    public void SetState(State state)
    {
        this.state = state;
    }

    private void HandleInputMovement()
    {
        float moveX = 0f;
        float moveZ = 0f;

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

        if (IsMoving(moveDirection))
        {
            playerMovementSystem.HandleMovement(moveDirection);
        }
    }

    private bool IsMoving(Vector3 moveDirection)
    {
        bool isMoving = moveDirection.magnitude >= Mathf.Epsilon;
        return isMoving;
    }

    private void HandleInputJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerJumpSystem.IsGrounded())
        {
            playerJumpSystem.HandleJump();
        }
    }

    private void HandleInputAttack()
    {
        if (!canAttack)
        {
            timerAttack -= Time.deltaTime;
            if (timerAttack <= 0f)
            {
                canAttack = true;
                timerAttack += timerAttackMax;
            }
        }

        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            canAttack = false;
            meleeAttackSystem.Attack();
        }
    }
}