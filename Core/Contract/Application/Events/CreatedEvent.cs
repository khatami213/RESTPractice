namespace Core.Contract.Application.Events;

public class CreatedEvent : Event
{
    public long Id { get; set; }
    public CreatedEvent(long id) => Id = id;
}
