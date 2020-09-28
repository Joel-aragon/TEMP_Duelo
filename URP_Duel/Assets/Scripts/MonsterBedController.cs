using System;
using UnityEngine;

public class MonsterBedController : MonoBehaviour
{
    private enum State
    {
        Play,
        Dead,
        Wait
    }

    private State state;

    [SerializeField] private float timerAttackMax = 3f;
    private float timerAttack;
    private bool canAttack = true;

    private MeleeAttackSystem meleeAttackSystem;
    private HealthSystem healthSystem;
    private Animator animator;
    private float timerDie = 3f;

    private void Awake()
    {
        meleeAttackSystem = GetComponent<MeleeAttackSystem>();
        healthSystem = GetComponent<HealthSystem>();
        animator = transform.Find("pfMonsterBedModel").GetComponent<Animator>();

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
        animator.SetTrigger("hit");
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        StoryboardUI.Instance.OnInactiveStoryboardUI -= StoryboardUI_OnInactiveStoryboardUI;

        PlayerController.OnPlayerDead -= PlayerController_OnPlayerDead;

        healthSystem.OnDamaged -= HealthSystem_OnDamaged;
        healthSystem.OnDied -= HealthSystem_OnDied;

        animator.SetTrigger("die");
        state = State.Dead;
    }

    private void StoryboardUI_OnInactiveStoryboardUI(object sender, EventArgs e)
    {
        state = State.Play;
    }

    private void PlayerController_OnPlayerDead(object sender, System.EventArgs e)
    {
        state = State.Wait;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Play:
                if (canAttack)
                {
                    animator.SetTrigger("attack");
                    canAttack = false;
                    meleeAttackSystem.Attack();
                }
                else
                {
                    timerAttack -= Time.deltaTime;
                    if (timerAttack <= 0f)
                    {
                        canAttack = true;
                        timerAttack += timerAttackMax;
                    }
                }
                break;

            case State.Dead:
                timerDie -= Time.deltaTime;
                {
                    if (timerDie <= 0f)
                    {
                        GameSceneManager.Load(GameSceneManager.Scene.AngerScene);
                    }
                }
                break;

            case State.Wait:
                break;
        }
    }

    private void OnDestroy()
    {
        StoryboardUI.Instance.OnInactiveStoryboardUI -= StoryboardUI_OnInactiveStoryboardUI;

        PlayerController.OnPlayerDead -= PlayerController_OnPlayerDead;
    }
}