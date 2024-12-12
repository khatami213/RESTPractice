namespace Core.ASP.Net.Infrastructure.JwtToken.Models;

public class JwtAuthResult
{
    public string AccessToken { get; set; }
    public RefreshToken RefreshToken { get; set; }
}
