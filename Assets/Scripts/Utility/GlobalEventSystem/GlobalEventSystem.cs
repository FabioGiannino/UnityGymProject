using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalEventSystem
{
    private static Action<GlobalEventArgs>[] gameEvents;

    /*creo l'array di actions dandogli come lunghezza il numero di possibili eventi definiti in EventName
          (Enum.GetValues(typeof(EventName)).Length -> è il numero di elementi definiti nell'enum)*/
    static GlobalEventSystem()
    {
        gameEvents = new Action<GlobalEventArgs>[Enum.GetValues(typeof(EventName)).Length];

    }

    public static void AddListener(EventName eventToListen, Action<GlobalEventArgs> listener)
    {
        gameEvents[(int)eventToListen] += listener;
    }

    public static void RemoveListener(EventName eventToListen, Action<GlobalEventArgs> listener)
    {
        gameEvents[(int)eventToListen] -= listener;
    }

    public static void CastEvent(EventName eventName, GlobalEventArgs message)
    {
        Debug.Log(eventName + GlobalEventArgsFactory.GetDebugString(eventName, message));
        gameEvents[(int)eventName]?.Invoke(message);
    }

}

public enum EventName
{
    LaunchPlayerStart,
    LaunchPlayerStop,
    PlayerHealthUpdate,
    PlayerDeath,
    PlayerUpdateLevelState
}