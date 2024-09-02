using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BaseState 
{
    

    [SerializeField]
    protected float maxTolerance;
    [SerializeField]
    protected float stateTimer;
    [SerializeField]
    protected StateEffect stateName;
    [SerializeField]
    protected float levelRecoveredPerSecond;

    protected float currentLevel;
    public bool IsAffected { get { return currentLevel >= maxTolerance; } }
    public float CurrentLevel { get { return currentLevel; } }
    public float MaxTolerance { get { return maxTolerance; } }
    public float StateTimer { get { return stateTimer; } }
    public StateEffect StateName {  get { return stateName; } }
    public Coroutine RecoveryCoroutine { get; set; }
    public Action OnStateEntered;
    public Action OnStateFinished;
    public Action<BaseState,float> OnLevelStateUpdate;


    public void ResetLevel()
    {
        currentLevel = 0;
    }
    public void UpdateLevelState(float levelAmount)
    {
        if (IsAffected) return;
        bool wasAffected = IsAffected;
        currentLevel = Math.Clamp(currentLevel +  levelAmount, 0, maxTolerance);
        OnLevelStateUpdate?.Invoke(this,currentLevel);
        if (!wasAffected && IsAffected)
        {
            OnStateEntered?.Invoke();
        }
    }
    
    public IEnumerator Recovery()
    {        
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        while (currentLevel>0)
        {
            currentLevel-= Time.deltaTime * levelRecoveredPerSecond;
            if(currentLevel < 0)
                currentLevel = 0;
            OnLevelStateUpdate?.Invoke(this,currentLevel);
            yield return wait;
        }
    }

}
