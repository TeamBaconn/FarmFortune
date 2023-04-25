using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    private static Dictionary<string, Action<EventParam>> eventDictionary = new Dictionary<string, Action<EventParam>>();

    public static void StartListening<T>(Action<EventParam> listener)
    {
        string name = typeof(T).Name;
        if (eventDictionary.TryGetValue(name, out Action<EventParam> thisEvent))
        {
            thisEvent += listener;
            eventDictionary[name] = thisEvent;
        }
        else
        {
            thisEvent += listener;
            eventDictionary.Add(name, thisEvent);
        }
    }

    public static void StopListening<T>(Action<EventParam> listener)
    {
        string name = typeof(T).Name;
        if (eventDictionary.TryGetValue(name, out Action<EventParam> thisEvent))
        {
            thisEvent -= listener;
            if (thisEvent == null)
            {
                eventDictionary.Remove(name);
            }
            else
            {
                eventDictionary[name] = thisEvent;
            }
        }
    }

    public static void TriggerEvent(EventParam message)
    {
        if (eventDictionary.TryGetValue(message.GetType().Name, out Action<EventParam> thisEvent))
        {
            thisEvent?.Invoke(message);
        }
    }
}