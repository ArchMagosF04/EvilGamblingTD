using UnityEngine;
using UnityEngine.Events;

public abstract class AbstractEventListener<T> : MonoBehaviour
{
    public AbstractEvent<T> eventToListen;
    public UnityEvent<T> onEventRaised;

    protected void OnEnable()
    {
        eventToListen?.Register(this);
    }

    protected void OnDisable()
    {
        eventToListen?.Unregister(this);
    }

    public virtual void RaiseEvent(T value)
    {
        onEventRaised?.Invoke(value);
    }
}
