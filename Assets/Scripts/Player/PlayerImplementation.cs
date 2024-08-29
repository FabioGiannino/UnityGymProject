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

    #region StateEffects

    [SerializeField]
    private StateEffectsModule stateEffectsModule;

    private Coroutine iceCoroutine;
    BaseStates fireState, frozenState, poisonState;

    private void InitStateEffectsModule()
    {
        fireState = stateEffectsModule.GetState(StateEffect.Fire);
        frozenState = stateEffectsModule.GetState(StateEffect.Cold);
        poisonState = stateEffectsModule.GetState(StateEffect.Poison);
        if (fireState != null)
        {
            fireState.OnStateEntered += OnFiredStateEntered;
            fireState.OnStateFinished += OnFiredStateFinished;
        }
        if (frozenState != null)
        {
            frozenState.OnStateEntered += OnFrozenStateEntered;
            frozenState.OnStateFinished += OnFrozenStateFinished;
        }
        if (poisonState != null)
        {
            poisonState.OnStateEntered += OnPoisonedStateEntered;
            poisonState.OnStateFinished += OnPoisonedStateFinished;
        }
        RestartStates();
        DebugStatesEffects();
    }

    private void RestartStates()
    {
        foreach (var state in stateEffectsModule.States)
        {
            state.ResetLevel();
        }
    }
    #region State: Frozen
    private void OnFrozenStateEntered()
    {
        if (iceCoroutine != null)
        {
            StopCoroutine(iceCoroutine);
            iceCoroutine = null;
        }
        else
        {
            playerController.OnFrozenStateEntered?.Invoke();    //don't slow the player for multiples times
        }
        iceCoroutine = StartCoroutine(StateEffectCoroutine(stateEffectsModule.GetState(StateEffect.Cold).StateTimer
                                , stateEffectsModule.GetState(StateEffect.Cold).OnStateFinished));
        NotifyStateUpdate(StateEffect.Cold);
    }
    private void OnFrozenStateFinished()
    {
        stateEffectsModule.GetState(StateEffect.Cold).ResetLevel();
        playerController.OnFrozenStateFinished?.Invoke();
        NotifyStateUpdate(StateEffect.Cold);
        iceCoroutine = null;
    }
    #endregion

    #region State: Fired
    private void OnFiredStateEntered()
    {

    }
    private void OnFiredStateFinished()
    {

    }
    #endregion

    #region State: Poisoned
    private void OnPoisonedStateEntered()
    {

    }
    private void OnPoisonedStateFinished()
    {

    }
    #endregion
    private void NotifyStateUpdate(StateEffect state)
    {
        //TODO: Creare l'evento per il global event system
    }

    private IEnumerator StateEffectCoroutine(float time, Action exitAction)
    {
        yield return new WaitForSeconds(time);
        exitAction?.Invoke();
    }
    private void DebugStatesEffects()
    {
        if (fireState != null)
        {
            fireState.OnStateEntered += () => { Debug.Log("Player IS ON FIRE!"); };
            fireState.OnStateFinished += () => { Debug.Log("Player is no more on fire..."); };
        }
        if (frozenState != null)
        {
            frozenState.OnStateEntered += () => { Debug.Log("Player IS FROZEN!"); };
            frozenState.OnStateFinished += () => { Debug.Log("Player is no more frozen..."); };
        }
        if (poisonState != null)
        {
            poisonState.OnStateEntered += () => { Debug.Log("Player IS POISONED"); };
            poisonState.OnStateFinished += () => { Debug.Log("Player is no more poisoned..."); };
        }
    }

    /*
    private Coroutine iceCoroutine;
    private Coroutine fireCoroutine;
    private Coroutine poisonCoroutine;

    private void RestartStates()
    {
        stateEffectsModule.ResetFireLevel();
        stateEffectsModule.ResetIceLevel();
        stateEffectsModule.ResetPoisonLevel();
    }


    private void OnFrozenStateEntered()
    {
        if (iceCoroutine != null)
        {
            StopCoroutine(iceCoroutine);
            iceCoroutine = null;
        }
        else
        {
            playerController.OnFrozenStateEntered?.Invoke();    //don't slow the player for multiples times
        }
        iceCoroutine = StartCoroutine(StateEffectCoroutine(stateEffectsModule.IceStateTimer,stateEffectsModule.OnFrozenStateFinished));
        NotifyStateUpdate(StateEffect.Cold);
    }
    private void OnFrozenStateFinished()
    {
        stateEffectsModule.ResetIceLevel();
        playerController.OnFrozenStateFinished?.Invoke();
        NotifyStateUpdate(StateEffect.Cold);
        iceCoroutine = null;
    }

    private void NotifyStateUpdate(StateEffect state)
    {
        //TODO: Creare l'evento per il global event system
    }

    private IEnumerator StateEffectCoroutine(float time, Action exitAction)
    {
        yield return new WaitForSeconds(time);
        exitAction?.Invoke();
    }

    #region DEBUG LOG
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


    */
    #endregion

}
