using System;

public class Subscription<T>
{
    private readonly EventAggregator eventAggregator;
    public Action<T> Action { get; set; }

    public Subscription(Action<T> _action, EventAggregator _eventAggregator)
    {
        Action = _action;
        eventAggregator = _eventAggregator;
    }

    public void Dispose()
    {

    }
}
