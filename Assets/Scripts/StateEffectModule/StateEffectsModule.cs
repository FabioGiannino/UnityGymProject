using Codice.Client.BaseCommands;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StateEffectsModule
{
    private const float timeToStartRecovery = 1.0f;

    [SerializeField]
    List<BaseState> states;

    public List<BaseState> States { get { return states; } }

    private MonoBehaviour controller;

    public void Init(MonoBehaviour controller)
    {
        this.controller = controller;
        foreach (var state in states)
        {
            state.ResetLevel();
        }
    }

    public BaseState GetState(StateEffect stateName)
    {
        return states.Find(state => state.StateName == stateName);
    }

    public float GetCurrentLevel(StateEffect stateName)
    {
        return GetState(stateName).CurrentLevel;        
    }

    public void TakeDamage(DamageContainer damage)
    {
        StateEffect stateEffect = GetStateEffectFromDamage(damage.DamageType);
        BaseState state = GetState(stateEffect);
        if (state == null) return;

        state.UpdateLevelState(damage.DamageAmount);
        if (state.IsAffected)
        {
            StopRecoveryCoroutine(state);
            state.RecoveryCoroutine = controller.StartCoroutine(WaitStateEffectTimer(state));
            return;
        }
        StopRecoveryCoroutine(state);
        state.RecoveryCoroutine = controller.StartCoroutine(WaitForRecovery(state));
    }

    private void StopRecoveryCoroutine(BaseState state)
    {
        if (state.RecoveryCoroutine == null) return;
        controller.StopCoroutine(state.RecoveryCoroutine);
        state.RecoveryCoroutine = null;
    }

    private StateEffect GetStateEffectFromDamage(DamageType damagetype)
    {
        switch (damagetype)
        {
            case DamageType.IceDamage:
                return StateEffect.Cold;
            case DamageType.FireDamage:
                return StateEffect.Fire;
            case DamageType.PoisonDamage:
                return StateEffect.Poison;
            default:
                return 0;
        }
    }

    private IEnumerator WaitForRecovery(BaseState state)
    {
        yield return new WaitForSeconds(timeToStartRecovery);
        state.RecoveryCoroutine = controller.StartCoroutine(state.Recovery());
    }

    private IEnumerator WaitStateEffectTimer(BaseState state)
    {
        yield return new WaitForSeconds(state.StateTimer);
        state.OnStateFinished?.Invoke();
        state.RecoveryCoroutine = controller.StartCoroutine(state.Recovery());
    }
}
/*
[Serializable]
public class StateEffectsModule
{
    [SerializeField]
    private float maxToleranceIce;
    [SerializeField]
    private float maxToleranceFire;
    [SerializeField]
    private float maxTolerancePoison;
    [SerializeField]
    private float iceStateTimer;
    [SerializeField]
    private float fireStateTimer;
    [SerializeField]
    private float poisonStateTimer;

    private float currentIceLevel;
    private float currentFireLevel;
    private float currentPoisonLevel;

    


    public float IceStateTimer { get { return iceStateTimer; } }


    public bool IsFrozen { get { return currentIceLevel > maxToleranceIce; } }
    public bool IsFired { get { return currentFireLevel > maxToleranceFire; } }
    public bool IsPoisoned { get { return currentPoisonLevel > maxTolerancePoison; } }


    public Action OnFrozenStateEntered;
    public Action OnFrozenStateFinished;
    public Action OnFiredStateEntered;
    public Action OnFiredStateFinished;
    public Action OnPoisonedStateEntered;
    public Action OnPoisonedStateFinished;


    public void ResetIceLevel()
    {
        currentIceLevel = 0;    
    }
    public void ResetFireLevel()
    {
        currentFireLevel = 0;
    }
    public void ResetPoisonLevel()
    {
        currentPoisonLevel = 0;
    }
    public Dictionary<StateEffect, float> GetCurrentStatesEffects()
    {
        Dictionary<StateEffect, float> statesLevel = new Dictionary<StateEffect, float>();
        statesLevel.Add(StateEffect.Cold, currentIceLevel);
        statesLevel.Add(StateEffect.Fire, currentFireLevel);
        statesLevel.Add(StateEffect.Poison, currentPoisonLevel);
        return statesLevel;
    }

    public void TakeDamage(DamageContainer damage)
    {
        Debug.Log("Player ha preso " + damage.DamageType.ToString() +" " + damage.DamageAmount);
        switch (damage.DamageType)
        {
            case DamageType.FireDamage:
                currentFireLevel += damage.DamageAmount;
                if (IsFired)
                    OnFiredStateEntered?.Invoke();                
                break;
            case DamageType.IceDamage:         
                currentIceLevel += damage.DamageAmount;
                if (IsFrozen)
                    OnFrozenStateEntered?.Invoke();
                break;
            case DamageType.PoisonDamage:
                currentPoisonLevel += damage.DamageAmount;
                if(IsPoisoned)
                    OnPoisonedStateEntered?.Invoke();
                break;

            default:
                return;
        }
    }

}
*/