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
    [SerializeField]
    private float postDamageInvulnerabilityTime;


    private Coroutine invulnerabilityCoroutine;

    private void InitHealthModule()
    {
        healthModule.OnDamageTaken += OnDamageTaken;
        healthModule.OnDeath += OnDeath;
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
        SetInvulnerable(postDamageInvulnerabilityTime);
    }
    public void OnDeath()       //richiamata dall'health module tramite l'action OnDeath
    {
        playerController.IsDeath = true;
        playerController.OnDeath?.Invoke();
        StopAllCoroutines();
    }
    private void NotifyHealthUpdate()
    {
        GlobalEventSystem.CastEvent(EventName.PlayerHealthUpdate, EventArgsFactory.PlayerHealthUpdateFactory(healthModule.MaxHealth, healthModule.CurrentHealth));
    }
    private void SetInvulnerable(float invulnerabilityTime)
    {
        if (invulnerabilityCoroutine != null)
        {
            StopCoroutine(invulnerabilityCoroutine);
        }
        invulnerabilityCoroutine = StartCoroutine(InvulnerabilityCoroutine(invulnerabilityTime));
    }
    private IEnumerator InvulnerabilityCoroutine(float invulnerabilityTime)
    {
        healthModule.IsInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityTime);
        healthModule.IsInvulnerable = false;
    }
    #endregion

    #region StateEffectModule
    [SerializeField]
    private StateEffectsModule stateEffectsModule;
    [SerializeField]
    private float fireDamageTimer;

    private Coroutine firedCoroutine;
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
        float timer = stateEffectsModule.GetState(StateEffect.Fire).StateTimer;
        firedCoroutine = StartCoroutine(FiredCoroutine(timer));        
        NotifyStateUpdate(StateEffect.Fire, true);
    }
    private void InternalFireStateFinished()
    {
        if (firedCoroutine != null)
        {
            StopCoroutine(firedCoroutine);
            firedCoroutine = null;
        }
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

    public IEnumerator FiredCoroutine(float timer)
    {
        DamageContainer damage = DamageContainer.DamageContainerFactory(DamageType.NormalDamage,3);
        while (true)
        {
            yield return new WaitForSeconds(fireDamageTimer);
            TakeDamage(damage);
        }
    }


    private void NotifyStateUpdate(StateEffect state, bool isAffected)
    {
        GlobalEventSystem.CastEvent(EventName.PlayerUpdateState, EventArgsFactory.PlayerUpdateStateFactory(state, isAffected));
    }
    #endregion
}
