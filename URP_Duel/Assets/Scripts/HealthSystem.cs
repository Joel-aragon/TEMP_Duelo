using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDamaged;
    public event EventHandler OnDied;

    [SerializeField] private int healthAmountMax;

    private int healthAmount;

    private void Awake()
    {
        healthAmount = healthAmountMax;
    }

    public void Damage(int damageAmount)
    {
        healthAmount -= damageAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, healthAmountMax);

        OnDamaged?.Invoke(this, EventArgs.Empty);

        if (IsDead())
        {
            OnDied?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsDead()
    {
        bool isDead = healthAmount == 0;
        return isDead;
    }

    public bool IsFullHealth()
    {
        bool isFullHealth = healthAmount == healthAmountMax;
        return isFullHealth;
    }

    public int GetHealthAmount()
    {
        return healthAmount;
    }

    public float GetHealthAmountNormalized()
    {
        float healthAmountNormalized = (float)healthAmount / healthAmountMax;
        return healthAmountNormalized;
    }
}