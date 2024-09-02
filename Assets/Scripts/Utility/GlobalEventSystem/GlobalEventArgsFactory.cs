using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class GlobalEventArgsFactory 
{


    private delegate string EventDebug(GlobalEventArgs message);
    private static Dictionary<EventName, EventDebug> methodDebugString;

    static GlobalEventArgsFactory()
    {
        methodDebugString = new Dictionary<EventName, EventDebug>();
        methodDebugString.Add(EventName.LaunchPlayerStart, new EventDebug(LaunchPlayerStartDebug));
        methodDebugString.Add(EventName.LaunchPlayerStop, new EventDebug(LaunchPlayerStopDebug));
        methodDebugString.Add(EventName.PlayerHealthUpdate, new EventDebug(PlayerHealthUpdateDebug));
        methodDebugString.Add(EventName.PlayerDeath, new EventDebug(PlayerDeathDebug)); 
        methodDebugString.Add(EventName.PlayerUpdateLevelState, new EventDebug(PlayerUpdateLevelStateDebug));
    }

    public static string GetDebugString(EventName eventType, GlobalEventArgs message)
    {
        return methodDebugString[eventType](message);
    }


    #region LaunchPlayerStart
    public static GlobalEventArgs LaunchPlayerStartFactory(float maxInputEvaluate)
    {
        GlobalEventArgs message = new GlobalEventArgs();
        message.variables = new object[1];
        message.variables[0] = maxInputEvaluate;
        return message;
    }
    public static void LaunchPlayerStartParser(GlobalEventArgs message, out float maxInputEvaluate)
    {
        maxInputEvaluate = (float)message.variables[0];
    }

    public static string LaunchPlayerStartDebug(GlobalEventArgs message)
    {
        return "LaunchPlayerStart Called: maxInputEvaluate = " + (float)message.variables[0];
    }
    #endregion

    #region LaunchPlayerStop
    public static GlobalEventArgs LaunchPlayerStopFactory()
    {
        GlobalEventArgs message = new GlobalEventArgs();        
        return message;
    }
    public static void LaunchPlayerStopParser(GlobalEventArgs message)
    {
        
    }

    public static string LaunchPlayerStopDebug(GlobalEventArgs message)
    {
        return "LaunchPlayerStop Called ";
    }
    #endregion

    #region PlayerHealthUpdate
    public static GlobalEventArgs PlayerHealthUpdateFactory(float maxHP, float currentHP)
    {
        GlobalEventArgs message = new GlobalEventArgs();
        message.variables = new object[2];
        message.variables[0] = maxHP;
        message.variables[1] = currentHP;
        return message;
    }
    public static void PlayerHealthUpdateParser(GlobalEventArgs message, out float maxHP, out float currentHP)
    {
        maxHP = (float)message.variables[0];
        currentHP = (float)message.variables[1];
    }

    public static string PlayerHealthUpdateDebug(GlobalEventArgs message)
    {
        return "PlayerHealthUpdate Called: maxHP = " + message.variables[0].ToString() + " currentHP= " +
            message.variables[1].ToString();
    }
    #endregion

    #region PlayerDeath
    public static GlobalEventArgs PlayerDeathFactory()
    {
        GlobalEventArgs message = new GlobalEventArgs();
        return message;
    }
    public static void PlayerDeathParser(GlobalEventArgs message)
    {

    }

    public static string PlayerDeathDebug(GlobalEventArgs message)
    {
        return "PlayerDeath Called ";
    }
    #endregion

    #region PlayerUpdateState
    public static GlobalEventArgs PlayerUpdateLevelStateFactory(StateEffect stateName, float stateLevelInPercentage)
    {
        GlobalEventArgs message = new GlobalEventArgs();
        message.variables = new object[2];
        message.variables[0]=stateName;
        message.variables[1]=stateLevelInPercentage;
        return message;
    }
    public static void PlayerUpdateLevelStateParser(GlobalEventArgs message,out StateEffect stateName, out float stateLevelInPercentage)
    {
        stateName = (StateEffect)message.variables[0];
        stateLevelInPercentage = (float)message.variables[1];
    }
    public static string PlayerUpdateLevelStateDebug(GlobalEventArgs message)
    {
        return "PlayerUpdateState Called: State "+ message.variables[0].ToString() +" - " + message.variables[1].ToString();
    }
    #endregion

   
}
