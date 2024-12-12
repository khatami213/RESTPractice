namespace Core.Contract.Application.Events;

public interface IEvent
{
    DateTime CreateDateTime { get; }
}
