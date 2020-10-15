using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class EventAggregator : MonoBehaviour
{
    private readonly Dictionary<Type, IList> subscribers = new Dictionary<Type, IList>();
    public static EventAggregator SingleInstance { get; private set; }

    private void Awake()
    {
        if(SingleInstance == null)
            SingleInstance = this;
    }

    public Subscription<T> Subscribe<T>(Action<T> action)
    {
        Subscription<T> sub = new Subscription<T>(action, SingleInstance);

        if(subscribers.TryGetValue(typeof(T), out IList actionList))
        {
            actionList.Add(sub);
        }
        else
        {
            actionList = new List<Subscription<T>>{ sub };
            subscribers.Add(typeof(T), actionList);
        }

        return sub;
    }

    public void Publish<T>(T message)
    {
        if (subscribers.ContainsKey(typeof(T)))
        {
            List<Subscription<T>> subs = new List<Subscription<T>>(subscribers[typeof(T)].Cast<Subscription<T>>());

            foreach(Subscription<T> s in subs)
            {
                s.Action?.Invoke(message);
            }
        }
    }

    public void Unsubscribe<T>(Subscription<T> sub)
    {
        if (subscribers[typeof(T)].Contains(sub))
        {
            int indexToRemove = subscribers[typeof(T)].IndexOf(sub);

            subscribers[typeof(T)].RemoveAt(indexToRemove);
        }
    }

    public void ClearAllSubscribers()
    {
        subscribers.Clear();
    }
}
