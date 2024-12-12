using Core.Contract.Application.Events;

namespace Core.Application.Events;

public class EventBus : IEventBus
{
    private readonly List<object> _subscribers = new();
    public EventBus()
    {

    }
    public void Publish<T>(T eventToPublish) where T : IEvent
    {
        var subscribers = _subscribers.OfType<Action<T>>().ToList();

        foreach (var eventHandler in subscribers)
            eventHandler.Invoke(eventToPublish);
    }

    public void Subscribe<T>(Action<T> eventHandler) where T : IEvent
    {
        _subscribers.Add(eventHandler);
    }

    public void Subscribe<T>(IEventHandler<T> eventHandler) where T : IEvent
    {
        _subscribers.Add(eventHandler);
    }
}
