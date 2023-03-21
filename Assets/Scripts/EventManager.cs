using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TypedEvent : UnityEvent<int, int> { }
public class TypedEvent2 : UnityEvent<int, int, float> { }
public class TypedEvent3 : UnityEvent<Vector3, int, float> { }

public class EventManager : MonoBehaviour
{
    private Dictionary<string, UnityEvent> eventDictionary;
    private Dictionary<string, TypedEvent> typedEventDictionary;
    private Dictionary<string, TypedEvent2> typedEvent2Dictionary;
    private Dictionary<string, TypedEvent3> typedEvent3Dictionary;

    private static EventManager eventManager;

    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    eventManager.Init();
                }
            }

            return eventManager;
        }
    }

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent>();
            typedEventDictionary = new Dictionary<string, TypedEvent>();
            typedEvent2Dictionary = new Dictionary<string, TypedEvent2>();
            typedEvent3Dictionary = new Dictionary<string, TypedEvent3>();
        }
    }

    public static void StartListening(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction listener)
    {
        if (eventManager == null) return;
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }

    public static void StartListening(string eventName, UnityAction<int, int> listener)
    {
        TypedEvent thisEvent = null;
        if (instance.typedEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new TypedEvent();
            thisEvent.AddListener(listener);
            instance.typedEventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction<int, int> listener)
    {
        if (eventManager == null) return;
        TypedEvent thisEvent = null;
        if (instance.typedEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName, int data1, int data2)
    {
        TypedEvent thisEvent = null;
        if (instance.typedEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(data1, data2);
        }
    }

    public static void StartListening(string eventName, UnityAction<int, int, float> listener)
    {
        TypedEvent2 thisEvent = null;
        if (instance.typedEvent2Dictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new TypedEvent2();
            thisEvent.AddListener(listener);
            instance.typedEvent2Dictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction<int, int, float> listener)
    {
        if (eventManager == null) return;
        TypedEvent2 thisEvent = null;
        if (instance.typedEvent2Dictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName, int data1, int data2, float data3)
    {
        TypedEvent2 thisEvent = null;
        if (instance.typedEvent2Dictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(data1, data2, data3);
        }
    }

    public static void StartListening(string eventName, UnityAction<Vector3, int, float> listener)
    {
        TypedEvent3 thisEvent = null;
        if (instance.typedEvent3Dictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new TypedEvent3();
            thisEvent.AddListener(listener);
            instance.typedEvent3Dictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction<Vector3, int, float> listener)
    {
        if (eventManager == null) return;
        TypedEvent3 thisEvent = null;
        if (instance.typedEvent3Dictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName, Vector3 data1, int data2, float data3)
    {
        TypedEvent3 thisEvent = null;
        if (instance.typedEvent3Dictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(data1, data2, data3);
        }
    }
}
