using System.Text.Json.Serialization;

namespace Core.Contract.Application.Events;

public class Event : IEvent
{
    [JsonIgnore]
    public DateTime CreateDateTime { get; private set; }

    public Event()
    {
        CreateDateTime = DateTime.Now;
    }
}
