using System;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : MonoBehaviour
{
    [SerializeField] private SerializedDictionary<String, UnityEvent> eventsToTrigger;

    public void TriggerEvent(String name)
    {
        if (eventsToTrigger.ContainsKey(name))
        {
            eventsToTrigger[name]?.Invoke();
        }
    }
}
