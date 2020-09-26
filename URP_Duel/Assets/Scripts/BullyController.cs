using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BullyController : MonoBehaviour
{
    private enum State
    {
        Chase,
        Search,
        Turn,
        Attack,
    }
    private State currentState;
    private State CurrentState
    {
        get
        {
            return currentState;
        }
        set
        {
            currentState = value;
            Debug.Log(value);
        }
    }

    [SerializeField] private Transform target;
    [SerializeField] private float searchDistance = 4f;
    [SerializeField] private float searchAngle = 10f;
    [SerializeField] private float timerAttackMax = 2f;
    [SerializeField] private float timerSearchMax = 0.5f;
    [SerializeField] private float turnSmoothTime = 0.5f;

    private NavMeshAgent navMeshAgent;
    private MeleeAttackSystem meleeAttackSystem;
    private float timerAttack;
    private bool canAttack = true;
    private float timerSearch;
    private bool canSearch = true;
    private float turnSmoothVelocity;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        meleeAttackSystem = GetComponent<MeleeAttackSystem>();

        CurrentState = State.Chase;

        timerAttack = timerAttackMax;
        timerSearch = timerSearchMax;
    }

    private void Update()
    {
        switch (CurrentState)
        {
            case State.Chase:
                navMeshAgent.SetDestination(target.position);
                if (Vector3.Distance(transform.position, target.position) <= searchDistance)
                {
                    CurrentState = State.Search;
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
                        CurrentState = State.Attack;
                    }
                    else
                    {
                        CurrentState = State.Turn;
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
                    CurrentState = State.Search;
                }

                if (Vector3.Distance(transform.position, target.position) > searchDistance)
                {
                    CurrentState = State.Chase;
                }
                break;

            case State.Attack:
                if (canAttack)
                {
                    canAttack = false;
                    meleeAttackSystem.Attack();
                }
                else
                {
                    CurrentState = State.Search;
                }
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
}