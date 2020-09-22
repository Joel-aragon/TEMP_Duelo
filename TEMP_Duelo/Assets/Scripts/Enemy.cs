using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum State
    {
        Chase,
        Prepare,
        Attack,
    }

    [SerializeField] Transform playerTransform;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] int attackDamage;
    [SerializeField] float distanceToAttack;
    [SerializeField] float hitRadius;
    [SerializeField] float attackTimerMax;

    private NavMeshAgent navMeshAgent;
    private HealthSystem healthSystem;
    private Transform attackTransform;
    private State state;
    private float attackTimer;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        healthSystem = GetComponent<HealthSystem>();
        attackTransform = transform.Find("attackTransform");

        if (navMeshAgent == null)
        {
            Debug.LogError("The Nav Mesh Agent is NULL.");
        }
        if (healthSystem == null)
        {
            Debug.LogError("The Health System is NULL.");
        }
        if (attackTransform == null)
        {
            Debug.LogError("The Attack Transform is NULL.");
        }

        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnDied += HealthSystem_OnDied;

        state = State.Chase;

        attackTimer = attackTimerMax;
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        Debug.Log("Damaging enemy: " + healthSystem.GetHealthAmount() + " hp left.");
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        switch (state)
        {
            case State.Chase:
                ChasePlayer();
                LookForPlayer();
                break;
            case State.Prepare:
                PrepareToAttack();
                break;
            case State.Attack:
                Attack();
                break;
        }
    }

    private void ChasePlayer()
    {
        navMeshAgent.SetDestination(playerTransform.position);
    }

    private void LookForPlayer()
    {
        // Checks distance to the Player
        if (Vector3.Distance(transform.position, playerTransform.position) <= distanceToAttack)
        {

            // Checks is the player is in front
            Collider[] playerHitColliderArray = Physics.OverlapSphere(attackTransform.position, hitRadius, playerLayer);
            foreach (Collider playerHitCollider in playerHitColliderArray)
            {
                if (playerHitCollider != null)
                {
                    state = State.Prepare;
                }
            }
        }
    }

    private void PrepareToAttack()
    {
        attackTimer -= Time.deltaTime;
        if(attackTimer <= 0f)
        {
            attackTimer += attackTimerMax;
            state = State.Attack;
        }
    }

    private void Attack()
    {
        Debug.Log("Attacking Player...");

        Collider[] playerHitColliderArray = Physics.OverlapSphere(attackTransform.position, hitRadius, playerLayer);
        foreach (Collider playerHitCollider in playerHitColliderArray)
        {
            if (playerHitCollider != null)
            {
                playerHitCollider.GetComponent<HealthSystem>().Damage(attackDamage);
            }
        }

        state = State.Chase;
    }

    private void OnDestroy()
    {
        Debug.Log("Destroying enemy...");
        healthSystem.OnDamaged -= HealthSystem_OnDamaged;
        healthSystem.OnDied -= HealthSystem_OnDied;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackTransform != null)
        {
            Gizmos.DrawWireSphere(attackTransform.position, hitRadius);
        }
    }
}
