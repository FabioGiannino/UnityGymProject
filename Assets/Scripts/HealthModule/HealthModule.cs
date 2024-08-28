using System;
using UnityEngine;

[Serializable]
public class HealthModule
{
    [SerializeField]
    private float maxHealth;

    private float currentHealth;


    public Action<DamageContainer> OnDamageTaken;
    public Action OnDeath;
    public bool IsInvulnerable {  get; set; }
    public bool IsAlive { get { return currentHealth > 0; } }
    public float MaxHealth {  get { return maxHealth; } }
    public float CurrentHealth { get { return currentHealth; } }


    public void RestartHealth()
    {
        currentHealth = maxHealth;
    }

    private bool CanTakeDamage()
    {
        return !IsInvulnerable || IsAlive;
    }

    public void TakeDamage(DamageContainer damage)
    {
        if (!CanTakeDamage()) return;
        currentHealth -= damage.DamageAmount;
        OnDamageTaken?.Invoke(damage);
        if (!IsAlive)
            OnDeath?.Invoke();       
    }

    
}
