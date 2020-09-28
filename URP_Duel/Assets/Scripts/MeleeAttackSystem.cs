using UnityEngine;

public class MeleeAttackSystem : MonoBehaviour
{
    [SerializeField] private int damageAmount;
    [SerializeField] private float hitRadius;
    [SerializeField] private LayerMask targetLayer;

    private Transform attackTransform;

    private void Awake()
    {
        attackTransform = transform.Find("attackTransform");
    }

    public void Attack()
    {
        Collider[] hits = Physics.OverlapSphere(attackTransform.position, hitRadius, targetLayer);
        foreach (Collider hit in hits)
        {
            if (hit != null)
            {
                hit.GetComponent<HealthSystem>().Damage(damageAmount);
            }
        }
    }

    public float GetHitRadius()
    {
        return hitRadius;
    }

    public Vector3 GetAttackTransformPosition()
    {
        return attackTransform.position;
    }

    public LayerMask GetTargetLayer()
    {
        return targetLayer;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackTransform != null)
        {
            Gizmos.DrawWireSphere(attackTransform.transform.position, hitRadius);
        }
    }
}