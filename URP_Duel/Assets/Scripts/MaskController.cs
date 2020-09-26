using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskController : MonoBehaviour
{
    private enum State
    {
        Happy,
        Sad
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
    [SerializeField] private float timerHappyFaceMax = 4f;
    [SerializeField] private float timerSadFaceMax = 6f;
    [SerializeField] private float turnSmoothTime = 0.2f;
    [SerializeField] private float timerRangeAttackMax = 0.5f;

    private RangeAttackSystem rangeAttackSystem;
    private float timerSadFace;
    private float timerHappyFace;
    private float turnSmoothVelocity;
    private float timerRangeAttack;
    private bool canRangeAttack = true;

    private void Awake()
    {
        rangeAttackSystem = GetComponent<RangeAttackSystem>();

        CurrentState = State.Sad;

        timerSadFace = timerSadFaceMax;
        timerHappyFace = timerHappyFaceMax;
    }

    private void Update()
    {
        switch (CurrentState)
        {
            case State.Sad:
                LookSadFace();
                RangeAttack();
                break;
            case State.Happy:
                LookHappyFace();
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
        if (CurrentState == State.Sad)
        {
            timerSadFace -= Time.deltaTime;
            if (timerSadFace <= 0f)
            {
                CurrentState = State.Happy;
                timerSadFace += timerSadFaceMax;
            }
        }
        if (CurrentState == State.Happy)
        {
            timerHappyFace -= Time.deltaTime;
            if (timerHappyFace <= 0f)
            {
                CurrentState = State.Sad;
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
}
