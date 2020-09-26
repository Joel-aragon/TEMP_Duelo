using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    [SerializeField] private int damageAmount;
    [SerializeField] private float hitRadius;
    [SerializeField] private LayerMask targetLayer;

    private Transform attackPosition;

    private void Awake()
    {
        attackPosition = transform.Find("attackPosition");
    }

    public void Attack()
    {
        Debug.Log(gameObject.name + " attacking...");
        Collider[] hits = Physics.OverlapSphere(attackPosition.transform.position, hitRadius, targetLayer);
        foreach (Collider hit in hits)
        {
            if (hit != null)
            {
                hit.GetComponent<HealthSystem>().Damage(damageAmount);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPosition != null)
        {
            Gizmos.DrawWireSphere(attackPosition.transform.position, hitRadius);
        }
    }
}
