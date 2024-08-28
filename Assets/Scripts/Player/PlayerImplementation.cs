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
        RestartStates();
        DebugStatesEffects();
    }

    
    #region HealthModule
    [SerializeField]
    private HealthModule healthModule;

    private void RestartHealth()
    {
        healthModule.RestartHealth();
        NotifyHealthUpdate();
    }

    #region ImplementedInterface: IDamageable
    public void TakeDamage(DamageContainer damage)
    {
        healthModule.TakeDamage(damage);
    }
    #endregion

    private void OnDamageTaken(DamageContainer damage)
    {
        playerController.OnDamageTaken?.Invoke(damage);
        NotifyHealthUpdate();
        stateEffectsModule.TakeDamage(damage);
        //TODO

    }
    private void NotifyHealthUpdate()
    {
        GlobalEventSystem.CastEvent(EventName.PlayerHealthUpdate, EventArgsFactory.PlayerHealthUpdateFactory(healthModule.MaxHealth, healthModule.CurrentHealth));
    }


    #endregion

    #region StateEffects    
    [SerializeField]
    private StateEffectsModule stateEffectsModule;
    private void RestartStates()
    {
        stateEffectsModule.ResetFireLevel();
        stateEffectsModule.ResetIceLevel();
        stateEffectsModule.ResetPoisonLevel();
    }

    private void DebugStatesEffects()
    {
        stateEffectsModule.OnFiredStateEntered += () => { Debug.Log("Player IS ON FIRE!"); };
        stateEffectsModule.OnFrozenStateEntered += () => { Debug.Log("Player IS FROZEN!"); };
        stateEffectsModule.OnPoisonedStateEntered += () => { Debug.Log("Player IS POISONED"); };

        stateEffectsModule.OnFiredStateFinished += () => { Debug.Log("Player is no more on fire..."); };
        stateEffectsModule.OnFrozenStateFinished += () => { Debug.Log("Player is no more frozen...");};
        stateEffectsModule.OnPoisonedStateFinished += () => { Debug.Log("Player is no more poisoned..."); };
    }


    #endregion

}
