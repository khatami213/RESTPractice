namespace Core.Contract.Application.Events;

public class EventModel
{
    public long Id { get; set; }

    public string EventName { get; set; }

    public DateTime DateTime { get; set; }

    public string Data { get; set; }

    public string UserName { get; set; }
}
