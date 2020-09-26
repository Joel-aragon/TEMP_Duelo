using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TearProjectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private int damageAmount;

    private Vector3 moveDirection;

    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    public static TearProjectile Create(Vector3 position, Transform target)
    {
        Transform pfTearProjectile = Resources.Load<Transform>("pfTearProjectile");
        Transform tearProjectileTransform = Instantiate(pfTearProjectile, position, Quaternion.identity);

        TearProjectile tearProjectile = tearProjectileTransform.GetComponent<TearProjectile>();
        tearProjectile.Setup(target);

        return tearProjectile;
    }

    private void Setup(Transform target)
    {
        moveDirection = (target.position - transform.position).normalized;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            HealthSystem healthSystem = other.GetComponent<HealthSystem>();
            if (healthSystem != null)
            {
                healthSystem.Damage(damageAmount);
                Destroy(gameObject);
            }
        }
    }
}
