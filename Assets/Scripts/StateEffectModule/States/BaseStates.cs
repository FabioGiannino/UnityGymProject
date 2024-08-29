using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BaseStates 
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
}
