using Core.Contract.Application.Events;

namespace Authorization.Command.Contract.Events.Users;

public class UserCreated : CreatedEvent
{
    public UserCreated(long id) : base(id)
    {
    }
}
