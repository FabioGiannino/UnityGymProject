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

    protected float currentLevel;
    public bool IsAffected { get { return currentLevel >= maxTolerance; } }
    public float CurrentLevel { get { return currentLevel; } set { currentLevel = value; } }
    public float StateTimer { get { return stateTimer; } }
    public StateEffect StateName {  get { return stateName; } }
    public Action OnStateEntered;
    public Action OnStateFinished;


    public void ResetLevel()
    {
        currentLevel = 0;
    }
    public void UpdateLevelState(float levelAmount)
    {
        bool wasAffected = IsAffected;
        currentLevel += levelAmount;
        if (!wasAffected && IsAffected)
        {
            OnStateEntered?.Invoke();
        }
    }

    public IEnumerator Recovery()
    {
        yield return new WaitForSeconds(stateTimer);
        OnStateFinished?.Invoke();
        ResetLevel();
    }

}
