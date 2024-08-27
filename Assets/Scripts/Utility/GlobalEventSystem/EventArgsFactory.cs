using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Codice.CM.Common.CmCallContext;

public static class EventArgsFactory 
{


    private delegate string EventDebug(EventArgs message);
    private static Dictionary<EventName, EventDebug> methodDebugString;

    static EventArgsFactory()
    {
        methodDebugString = new Dictionary<EventName, EventDebug>();
        methodDebugString.Add(EventName.LaunchPlayerStart, new EventDebug(LaunchPlayerStartDebug));
        methodDebugString.Add(EventName.LaunchPlayerStop, new EventDebug(LaunchPlayerStopDebug));
        methodDebugString.Add(EventName.PlayerHealthUpdate, new EventDebug(PlayerHealthUpdateDebug));
        methodDebugString.Add(EventName.PlayerDeath, new EventDebug(PlayerDeathDebug)); 
    }

    public static string GetDebugString(EventName eventType, EventArgs message)
    {
        return methodDebugString[eventType](message);
    }


    #region LaunchPlayerStart
    public static EventArgs LaunchPlayerStartFactory(float maxInputEvaluate)
    {
        EventArgs message = new EventArgs();
        message.variables = new object[1];
        message.variables[0] = maxInputEvaluate;
        return message;
    }
    public static void LaunchPlayerStartParser(EventArgs message, out float maxInputEvaluate)
    {
        maxInputEvaluate = (float)message.variables[0];
    }

    public static string LaunchPlayerStartDebug(EventArgs message)
    {
        return "LaunchPlayerStart Called: maxInputEvaluate = " + (float)message.variables[0];
    }
    #endregion

    #region LaunchPlayerStop
    public static EventArgs LaunchPlayerStopFactory()
    {
        EventArgs message = new EventArgs();        
        return message;
    }
    public static void LaunchPlayerStopParser(EventArgs message)
    {
        
    }

    public static string LaunchPlayerStopDebug(EventArgs message)
    {
        return "LaunchPlayerStop Called ";
    }
    #endregion

    #region PlayerHealthUpdate
    public static EventArgs PlayerHealthUpdateFactory(float maxHP, float currentHP)
    {
        EventArgs message = new EventArgs();
        message.variables = new object[2];
        message.variables[0] = maxHP;
        message.variables[1] = currentHP;
        return message;
    }
    public static void PlayerHealthUpdateParser(EventArgs message, out float maxHP, out float currentHP)
    {
        maxHP = (float)message.variables[0];
        currentHP = (float)message.variables[1];
    }

    public static string PlayerHealthUpdateDebug(EventArgs message)
    {
        return "PlayerHealthUpdate Called: maxHP = " + message.variables[0].ToString() + " currentHP= " +
            message.variables[1].ToString();
    }
    #endregion

    #region PlayerDeath
    public static EventArgs PlayerDeathFactory()
    {
        EventArgs message = new EventArgs();
        return message;
    }
    public static void PlayerDeathParser(EventArgs message)
    {

    }

    public static string PlayerDeathDebug(EventArgs message)
    {
        return "PlayerDeath Called ";
    }
    #endregion

}
