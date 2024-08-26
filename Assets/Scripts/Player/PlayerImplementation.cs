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
        healthModule.RestartHealth();
        healthModule.OnDamageTaken += OnDamageTaken;
    }




    #region HealthModule
    [SerializeField]
    private HealthModule healthModule;

    public void TakeDamage(DamageContainer damage)
    {
        healthModule.TakeDamage(damage);
    }

    private void OnDamageTaken(DamageContainer damage)
    {
        playerController.OnDamageTaken?.Invoke(damage);
        
        //TODO

    }
    #endregion

}
