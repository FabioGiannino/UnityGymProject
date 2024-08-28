using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerImplementation : MonoBehaviour, IDamageable
{
    [SerializeField]
    private PlayerController playerController;


    private void Start()
    {
        healthModule.OnDamageTaken += OnDamageTaken;
        RestartHealth();
    }


    #region HealthModule
    [SerializeField]
    private HealthModule healthModule;

    private void RestartHealth()
    {
        healthModule.RestartHealth();
        NotifyHealthUpdate();
    }

    public void TakeDamage(DamageContainer damage)
    {
        healthModule.TakeDamage(damage);
    }

    private void OnDamageTaken(DamageContainer damage)
    {
        playerController.OnDamageTaken?.Invoke(damage);
        NotifyHealthUpdate();
        //TODO

    }
    private void NotifyHealthUpdate()
    {
        GlobalEventSystem.CastEvent(EventName.PlayerHealthUpdate, EventArgsFactory.PlayerHealthUpdateFactory(healthModule.MaxHealth, healthModule.CurrentHealth));
    }
    #endregion

}
