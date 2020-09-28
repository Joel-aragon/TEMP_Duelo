using UnityEngine;
using System;

public class MaskController : MonoBehaviour
{
    private enum State
    {
        Happy,
        Sad,
        Dead,
        Wait
    }
    private State state;

    [SerializeField] private Transform target;
    [SerializeField] private float timerHappyFaceMax = 4f;
    [SerializeField] private float timerSadFaceMax = 6f;
    [SerializeField] private float turnSmoothTime = 0.2f;
    [SerializeField] private float timerRangeAttackMax = 0.5f;

    private RangeAttackSystem rangeAttackSystem;
    private HealthSystem healthSystem;
    private Animator animator;
    private GameObject maskParticles;
    private float timerSadFace;
    private float timerHappyFace;
    private float turnSmoothVelocity;
    private float timerRangeAttack;
    private bool canRangeAttack = true;
    private float timerDie = 3f;

    private void Awake()
    {
        rangeAttackSystem = GetComponent<RangeAttackSystem>();
        healthSystem = GetComponent<HealthSystem>();
        animator = transform.Find("pfMaskModel").GetComponent<Animator>();
        maskParticles = transform.Find("pfMaskParticles").gameObject;

        timerSadFace = timerSadFaceMax;
        timerHappyFace = timerHappyFaceMax;

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
        ParticleSystemManager.Instance.CreateParticle(ParticleSystemManager.Particle.pfShadowParticles, transform.position);
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        animator.SetTrigger("die");
        maskParticles.SetActive(false);

        StoryboardUI.Instance.OnInactiveStoryboardUI -= StoryboardUI_OnInactiveStoryboardUI;

        PlayerController.OnPlayerDead -= PlayerController_OnPlayerDead;

        healthSystem.OnDamaged -= HealthSystem_OnDamaged;
        healthSystem.OnDied -= HealthSystem_OnDied;

        state = State.Dead;
    }

    private void StoryboardUI_OnInactiveStoryboardUI(object sender, EventArgs e)
    {
        state = State.Happy;
    }

    private void PlayerController_OnPlayerDead(object sender, System.EventArgs e)
    {
        state = State.Wait;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Sad:
                LookSadFace();
                RangeAttack();
                break;

            case State.Happy:
                LookHappyFace();
                break;

            case State.Dead:
                timerDie -= Time.deltaTime;
                {
                    if (timerDie <= 0f)
                    {
                        GameSceneManager.Load(GameSceneManager.Scene.AcceptanceScene);
                    }
                }
                break;

            case State.Wait:
                break;
        }

        ChangeState();
        TimerRangeAttack();
    }

    private void LookSadFace()
    {
        Vector3 lookDirection = (target.position - transform.position).normalized;
        float targetAngle = Mathf.Atan2(lookDirection.x, lookDirection.z) * Mathf.Rad2Deg;
        float smoothedTargetAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, smoothedTargetAngle, 0f);
    }

    private void LookHappyFace()
    {
        Vector3 lookDirection = (transform.position - target.position).normalized;
        float targetAngle = Mathf.Atan2(lookDirection.x, lookDirection.z) * Mathf.Rad2Deg;
        float smoothedTargetAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, smoothedTargetAngle, 0f);
    }

    private void RangeAttack()
    {
        if (canRangeAttack && target != null)
        {
            canRangeAttack = false;
            rangeAttackSystem.Attack(target);
        }
    }

    private void ChangeState()
    {
        if (state == State.Sad)
        {
            timerSadFace -= Time.deltaTime;
            if (timerSadFace <= 0f)
            {
                SoundManager.Instance.PlaySound(SoundManager.Sound.maskLaugh, transform.position);
                state = State.Happy;
                timerSadFace += timerSadFaceMax;
            }
        }
        if (state == State.Happy)
        {
            timerHappyFace -= Time.deltaTime;
            if (timerHappyFace <= 0f)
            {
                state = State.Sad;
                timerHappyFace += timerHappyFaceMax;
            }
        }
    }

    private void TimerRangeAttack()
    {
        timerRangeAttack -= Time.deltaTime;
        if (timerRangeAttack <= 0f)
        {
            canRangeAttack = true;
            timerRangeAttack += timerRangeAttackMax;
        }
    }

    private void OnDestroy()
    {
        StoryboardUI.Instance.OnInactiveStoryboardUI -= StoryboardUI_OnInactiveStoryboardUI;

        PlayerController.OnPlayerDead -= PlayerController_OnPlayerDead;
    }
}