using Core.Contract.Application.Commands;
using Core.DataAnnotations;

namespace Authorization.Command.Contract.Commands.Users;

public class CreateUser : ICommand
{
    [CustomRequired]
    public string Username { get; set; }

    [CustomRequired]
    public string Password { get; set; }

    [CustomRequired]
    public string ConfirmPassword { get; set; }
    
    public string Email { get; set; }
}
