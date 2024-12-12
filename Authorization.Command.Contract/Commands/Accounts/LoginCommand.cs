using Core.Contract.Application.Commands;
using Core.DataAnnotations;

namespace Authorization.Command.Contract.Commands.Accounts;

public class LoginCommand : ICommand
{
    [CustomRequired]
    public string Username { get; set; }

    [CustomRequired]
    public string Password { get; set; }
}
