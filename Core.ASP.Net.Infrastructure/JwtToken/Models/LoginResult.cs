namespace Core.ASP.Net.Infrastructure.JwtToken.Models;

public class LoginResult
{
    public string? Username { get; set; }
    public string? Role { get; set; }
    public string? AccessToken { get; set; }
    public RefreshToken? RefreshToken { get; set; }
}
