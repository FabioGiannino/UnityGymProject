using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class GlobalEventSystem
{
    private static Action<EventArgs>[] gameEvents;

    /*creo l'array di actions dandogli come lunghezza il numero di possibili eventi definiti in EventName
          (Enum.GetValues(typeof(EventName)).Length -> è il numero di elementi definiti nell'enum)*/
    static GlobalEventSystem()
    {
        gameEvents = new Action<EventArgs>[Enum.GetValues(typeof(EventName)).Length];

    }

    //si registra alla specifica action dello specifico Evento, un action passato dall'esterno
    public static void AddListener(EventName eventToListen, Action<EventArgs> listener)
    {
        gameEvents[(int)eventToListen] += listener;
    }

    //cosi come è possibile registrare un action, deve essere possibile anche rimuoverlo
    public static void RemoveListener(EventName eventToListen, Action<EventArgs> listener)
    {
        gameEvents[(int)eventToListen] -= listener;
    }

    //Lancio l'action passandogli il nome dell'evento da lanciare e gli argomenti
    public static void CastEvent(EventName eventName, EventArgs message)
    {
        Debug.Log(eventName + EventArgsFactory.GetDebugString(eventName, message));
        gameEvents[(int)eventName]?.Invoke(message);
    }

}

public enum EventName
{
    LaunchPlayerStart,
    LaunchPlayerStop
}