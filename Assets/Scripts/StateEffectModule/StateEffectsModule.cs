using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public float CurrentIceLevel { get { return currentIceLevel; } }
    public float CurrentFireLevel { get { return currentFireLevel; } }
    public float CurrentPoisonLevel { get { return currentPoisonLevel; } }

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
