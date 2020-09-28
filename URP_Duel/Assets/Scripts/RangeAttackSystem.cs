using UnityEngine;

public class RangeAttackSystem : MonoBehaviour
{
    private Transform attackTransform;

    private void Awake()
    {
        attackTransform = transform.Find("attackTransform");
    }

    public void Attack(Transform target)
    {
        TearProjectile.Create(attackTransform.position, target);
    }
}