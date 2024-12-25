using Authorization.Application.Exceptions.Accounts;
using Authorization.Command.Contract.Commands.Accounts;
using Core.ASP.Net.Infrastructure.JwtToken.Abstractions;
using Core.ASP.Net.Infrastructure.JwtToken.Models;
using Core.Contract.Application.Commands;
using System.Security.Claims;
using System.Text;

namespace Authorization.Application.Services.CommandHandlers.Accounts;

public class LoginHandler : ICommandHandler<LoginCommand, LoginResult>
{
    private readonly IUserService _userService;
    private readonly IJwtAuthManager _jwtAuthManager;

    public LoginHandler(IUserService userService, IJwtAuthManager jwtAuthManager)
    {
        _userService = userService;
        _jwtAuthManager = jwtAuthManager;
    }

    public async Task<LoginResult> Handle(LoginCommand command)
    {
        var user = await _userService.IsValidUserCredentials(command.Username, command.Password);
        if (user == null)
            throw new InvalidUsernameOrPasswordException();

        var roleStr = new StringBuilder();
        var roles = await _userService.GetUserRole(command.Username);
        foreach (var role in roles)
            roleStr.Append($"{role},");

        var claims = new[]
        {
                new Claim(ClaimTypes.Name,command.Username),
                new Claim(ClaimTypes.Role, roleStr.ToString()),
                new Claim("UserId", user.Id.ToString()),
        };

        var jwtResult = _jwtAuthManager.GenerateTokens(command.Username, claims, DateTime.Now);

        return new LoginResult()
        {
            Username = user.Username,
            Role = roles.ToList(), //roleStr.ToString(),
            AccessToken = jwtResult.AccessToken,
            RefreshToken = new RefreshTokenResult
            {
                Token = jwtResult.RefreshToken.Token,
                ExpiryDate = jwtResult.RefreshToken.ExpiryDate,
            }
        };
    }
}
