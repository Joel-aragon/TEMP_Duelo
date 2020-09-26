using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBedController : MonoBehaviour
{
    [SerializeField] private float timerAttackMax = 3f;
    private float timerAttack;
    private bool canAttack = true;

    private CombatSystem combatSystem;

    private void Awake()
    {
        combatSystem = GetComponent<CombatSystem>();
    }

    private void Update()
    {
        if (canAttack)
        {
            canAttack = false;
            combatSystem.Attack();
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
