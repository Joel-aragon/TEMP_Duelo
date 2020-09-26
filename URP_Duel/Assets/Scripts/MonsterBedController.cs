using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBedController : MonoBehaviour
{
    [SerializeField] private float timerAttackMax = 3f;
    private float timerAttack;
    private bool canAttack = true;

    private MeleeAttackSystem meleeAttackSystem;

    private void Awake()
    {
        meleeAttackSystem = GetComponent<MeleeAttackSystem>();
    }

    private void Update()
    {
        if (canAttack)
        {
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
    }
}
