namespace Core.Contract.Application.Events;

public class DeletedEvent : Event
{
    public long Id { get; set; }
    public DeletedEvent(long id) => Id = id;
}
