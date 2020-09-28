using UnityEngine;
using UnityEngine.AI;
using System;

public class BullyController : MonoBehaviour
{
    private enum State
    {
        Chase,
        Search,
        Turn,
        Attack,
        Dead,
        Wait
    }
    private State state;

    [SerializeField] private Transform target;
    [SerializeField] private float searchDistance = 4f;
    [SerializeField] private float searchAngle = 10f;
    [SerializeField] private float timerAttackMax = 2f;
    [SerializeField] private float timerSearchMax = 0.5f;
    [SerializeField] private float turnSmoothTime = 0.5f;

    private NavMeshAgent navMeshAgent;
    private MeleeAttackSystem meleeAttackSystem;
    private HealthSystem healthSystem;
    private Animator animator;
    private float timerAttack;
    private bool canAttack = true;
    private float timerSearch;
    private bool canSearch = true;
    private float turnSmoothVelocity;
    private float timerDie = 3f;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        meleeAttackSystem = GetComponent<MeleeAttackSystem>();
        healthSystem = GetComponent<HealthSystem>();
        animator = transform.Find("pfBullyModelAltAlt").GetComponent<Animator>();

        timerAttack = timerAttackMax;
        timerSearch = timerSearchMax;

        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnDied += HealthSystem_OnDied;

        state = State.Wait;
    }

    private void Start()
    {
        StoryboardUI.Instance.OnInactiveStoryboardUI += StoryboardUI_OnInactiveStoryboardUI;

        PlayerController.OnPlayerDead += PlayerController_OnPlayerDead;
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.bullyDamaged, SoundManager.Sound.bullyDamagedAlt, transform.position);
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.bullyDie, transform.position);

        StoryboardUI.Instance.OnInactiveStoryboardUI -= StoryboardUI_OnInactiveStoryboardUI;

        PlayerController.OnPlayerDead -= PlayerController_OnPlayerDead;

        healthSystem.OnDamaged -= HealthSystem_OnDamaged;
        healthSystem.OnDied -= HealthSystem_OnDied;

        animator.SetTrigger("die");
        state = State.Dead;
    }

    private void StoryboardUI_OnInactiveStoryboardUI(object sender, EventArgs e)
    {
        state = State.Chase;
    }

    private void PlayerController_OnPlayerDead(object sender, System.EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.bullyDefault, transform.position);
        state = State.Wait;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Chase:
                animator.SetBool("isWalking", true);
                navMeshAgent.SetDestination(target.position);
                if (Vector3.Distance(transform.position, target.position) <= searchDistance)
                {
                    state = State.Search;
                }
                break;

            case State.Search:
                if (canSearch)
                {
                    canSearch = false;

                    bool foundPlayer = false;
                    Collider[] hits = Physics.OverlapSphere(meleeAttackSystem.GetAttackTransformPosition(), meleeAttackSystem.GetHitRadius(), meleeAttackSystem.GetTargetLayer());
                    foreach (Collider hit in hits)
                    {
                        if (hit != null)
                        {
                            foundPlayer = true;
                        }
                    }
                    if (foundPlayer)
                    {
                        state = State.Attack;
                    }
                    else
                    {
                        state = State.Turn;
                    }
                }
                break;

            case State.Turn:
                Vector3 lookDirection = (target.position - transform.position).normalized;
                float targetAngle = Mathf.Atan2(lookDirection.x, lookDirection.z) * Mathf.Rad2Deg;
                float smoothedTargetAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, smoothedTargetAngle, 0f);
                if (Vector3.Angle(transform.forward, lookDirection) < searchAngle)
                {
                    state = State.Search;
                }

                if (Vector3.Distance(transform.position, target.position) > searchDistance)
                {
                    SoundManager.Instance.PlaySound(SoundManager.Sound.bullyTaunt, SoundManager.Sound.bullyTauntAlt, transform.position);
                    state = State.Chase;
                }
                break;

            case State.Attack:
                if (canAttack)
                {
                    SoundManager.Instance.PlaySound(SoundManager.Sound.bullyAttack, SoundManager.Sound.bullyAttackAlt, transform.position);
                    animator.SetBool("isWalking", false);
                    animator.SetTrigger("attack");
                    canAttack = false;
                    meleeAttackSystem.Attack();
                }
                else
                {
                    state = State.Search;
                }
                break;

            case State.Dead:
                timerDie -= Time.deltaTime;
                {
                    if (timerDie <= 0f)
                    {
                        GameSceneManager.Load(GameSceneManager.Scene.NegotiationScene);
                    }
                }
                break;

            case State.Wait:
                break;
        }

        TimerSearch();
        TimerAttack();
    }

    private void TimerSearch()
    {
        if (!canSearch)
        {
            timerSearch -= Time.deltaTime;
            if (timerSearch <= 0f)
            {
                canSearch = true;
                timerSearch += timerSearchMax;
            }
        }
    }

    private void TimerAttack()
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
    }

    private void OnDestroy()
    {
        StoryboardUI.Instance.OnInactiveStoryboardUI -= StoryboardUI_OnInactiveStoryboardUI;

        PlayerController.OnPlayerDead -= PlayerController_OnPlayerDead;
    }
}