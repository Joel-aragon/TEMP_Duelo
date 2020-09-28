using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static EventHandler OnPlayerDead;

    private enum State
    {
        Play,
        Dead,
        Wait
    }

    private State state;

    [SerializeField] private float timerAttackMax = 1f;
    private float timerDie = 3f;
    private float timerAttack;
    private bool canAttack = true;

    private PlayerMovementSystem playerMovementSystem;
    private PlayerJumpSystem playerJumpSystem;
    private MeleeAttackSystem meleeAttackSystem;
    private HealthSystem healthSystem;
    private Animator animator;

    private void Awake()
    {
        playerMovementSystem = GetComponent<PlayerMovementSystem>();
        playerJumpSystem = GetComponent<PlayerJumpSystem>();
        meleeAttackSystem = GetComponent<MeleeAttackSystem>();
        healthSystem = GetComponent<HealthSystem>();
        animator = transform.Find("pfPlayerModelAltAlt").GetComponent<Animator>();

        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnDied += HealthSystem_OnDied;

        state = State.Wait;
    }

    private void Start()
    {
        StoryboardUI.Instance.OnInactiveStoryboardUI += StoryboardUI_OnInactiveStoryboardUI;

        OptionsUI.Instance.OnActiveOptionsUI += OptionsUI_OnActiveOptionsUI;
        OptionsUI.Instance.OnInactiveOptionsUI += OptionsUI_OnInactiveOptionsUI;
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.playerDamaged, SoundManager.Sound.playerDamagedAlt, transform.position);
    }

    private void HealthSystem_OnDied(object sender, EventArgs e)
    {
        healthSystem.OnDamaged -= HealthSystem_OnDamaged;
        healthSystem.OnDied -= HealthSystem_OnDied;

        StoryboardUI.Instance.OnInactiveStoryboardUI -= StoryboardUI_OnInactiveStoryboardUI;

        OptionsUI.Instance.OnActiveOptionsUI -= OptionsUI_OnActiveOptionsUI;
        OptionsUI.Instance.OnInactiveOptionsUI -= OptionsUI_OnInactiveOptionsUI;

        state = State.Dead;
        animator.SetTrigger("die");

        OnPlayerDead?.Invoke(this, EventArgs.Empty);
    }

    private void StoryboardUI_OnInactiveStoryboardUI(object sender, EventArgs e)
    {
        state = State.Play;
    }

    private void OptionsUI_OnActiveOptionsUI(object sender, EventArgs e)
    {
        state = State.Wait;
    }

    private void OptionsUI_OnInactiveOptionsUI(object sender, EventArgs e)
    {
        state = State.Play;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Play:
                HandleInputMovement();
                HandleInputJump();
                HandleInputAttack();
                break;

            case State.Dead:
                timerDie -= Time.deltaTime;
                {
                    if (timerDie <= 0f)
                    {
                        state = State.Wait;
                        GameOverUI.Instance.Show();
                    }
                }
                break;

            case State.Wait:
                break;
        }
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
            if (playerJumpSystem.IsGrounded())
            {
                animator.SetBool("isWalking", true);
            }
            playerMovementSystem.HandleMovement(moveDirection);
        }
        else
        {
            animator.SetBool("isWalking", false);
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
            SoundManager.Instance.PlaySound(SoundManager.Sound.playerJump, SoundManager.Sound.playerJumpAlt, transform.position);
            animator.SetTrigger("jump");
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
            SoundManager.Instance.PlaySound(SoundManager.Sound.playerAttack, SoundManager.Sound.playerAttackAlt, transform.position);
            animator.SetTrigger("attack");
            canAttack = false;
            meleeAttackSystem.Attack();
        }
    }

    private void OnDestroy()
    {
        StoryboardUI.Instance.OnInactiveStoryboardUI -= StoryboardUI_OnInactiveStoryboardUI;

        OptionsUI.Instance.OnActiveOptionsUI -= OptionsUI_OnActiveOptionsUI;
        OptionsUI.Instance.OnInactiveOptionsUI -= OptionsUI_OnInactiveOptionsUI;
    }
}