namespace Core.Contract.Application.Events;

public class UpdatedEvent : Event
{
    public long Id { get; set; }
    public UpdatedEvent(long id) => Id = id;
}
