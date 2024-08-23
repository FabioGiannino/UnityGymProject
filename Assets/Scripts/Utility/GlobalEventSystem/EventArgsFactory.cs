using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventArgsFactory 
{


    private delegate string EventDebug(EventArgs message);
    private static Dictionary<EventName, EventDebug> methodDebugString;

    static EventArgsFactory()
    {
        methodDebugString = new Dictionary<EventName, EventDebug>();
        methodDebugString.Add(EventName.LaunchPlayerStart, new EventDebug(LaunchPlayerStartDebug));
        methodDebugString.Add(EventName.LaunchPlayerStop, new EventDebug(LaunchPlayerStopDebug));
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
        return "LaunchPlayerStartCalled: maxInputEvaluate = " + (float)message.variables[0];
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
        return "LaunchPlayerStopCalled ";
    }
    #endregion

}
