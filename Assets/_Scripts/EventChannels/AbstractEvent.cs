using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractEvent<T> : ScriptableObject
{
    public T LastValue { get ; private set; }
    protected List<AbstractEventListener<T>> listeners = new List<AbstractEventListener<T>>();

    public void Register(AbstractEventListener<T> listener)
    {
        if (!listeners.Contains(listener)) listeners.Add(listener);
    }

    public void Unregister(AbstractEventListener<T> listener)
    {
        if (listeners.Contains(listener)) listeners.Remove(listener);
    }

    public void InvokeEvent(T value)
    {
        LastValue = value;

        for (int i = 0; i < listeners.Count; i++)
        {
            listeners[i].RaiseEvent(value);
        }

        //foreach (AbstractEventListener<T> listener in listeners)
        //{
        //    listener.RaiseEvent(value);
        //}
    }
}
