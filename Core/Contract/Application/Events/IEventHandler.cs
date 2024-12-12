namespace Core.Contract.Application.Events;

public interface IEventHandler<in T> where T : IEvent
{
    void Handle(T eventToHandle);
}