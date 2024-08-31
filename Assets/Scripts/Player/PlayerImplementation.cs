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
        InitHealthModule();
        InitStateEffectsModule();
    }

    #region HealthModule
    [SerializeField]
    private HealthModule healthModule;

    private void InitHealthModule()
    {
        healthModule.OnDamageTaken += OnDamageTaken;
        RestartHealth();
    }

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

    #region StateEffectModule
    [SerializeField]
    private StateEffectsModule stateEffectsModule;


    private void InitStateEffectsModule()
    {
        stateEffectsModule.Init(this);
        foreach (var state in stateEffectsModule.States)
        {
            switch (state.StateName)
            {
                case StateEffect.Cold:
                    state.OnStateEntered += InternalColdStateEntered;
                    state.OnStateFinished += InternalColdStateFinished;
                    break;
                case StateEffect.Fire:
                    state.OnStateEntered += InternalFireStateEntered;
                    state.OnStateFinished += InternalFireStateFinished;
                    break;
                case StateEffect.Poison:
                    state.OnStateEntered += InternalPoisonStateEntered;
                    state.OnStateFinished += InternalPoisonStateFinished;
                    break;
                default:
                    return;
            }
        }
    }

    private void InternalColdStateEntered()
    {
        playerController.OnFrozenStateEntered?.Invoke();
        NotifyStateUpdate(StateEffect.Cold, true);
    }
    private void InternalColdStateFinished()
    {
        playerController.OnFrozenStateFinished?.Invoke();
        NotifyStateUpdate(StateEffect.Cold, false);
    }
    private void InternalFireStateEntered()
    {
        NotifyStateUpdate(StateEffect.Fire, true);
    }
    private void InternalFireStateFinished()
    {
        NotifyStateUpdate(StateEffect.Fire, false);
    }
    private void InternalPoisonStateEntered()
    {
        NotifyStateUpdate(StateEffect.Poison, true);
    }
    private void InternalPoisonStateFinished()
    {
        NotifyStateUpdate(StateEffect.Poison, false);
    }

    private void NotifyStateUpdate(StateEffect state, bool isAffected)
    {
        GlobalEventSystem.CastEvent(EventName.PlayerUpdateState, EventArgsFactory.PlayerUpdateStateFactory(state, isAffected));
    }
    #endregion
}
