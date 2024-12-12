namespace Core.Contract.Application.Events;

public interface IEventBus
{
    void Subscribe<T>(Action<T> eventHandler) where T : IEvent;
    void Subscribe<T>(IEventHandler<T> eventHandler) where T : IEvent;
    void Publish<T>(T eventToPublish) where T : IEvent;
}