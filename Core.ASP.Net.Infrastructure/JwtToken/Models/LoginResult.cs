namespace Core.ASP.Net.Infrastructure.JwtToken.Models;

public class LoginResult
{
    public string? Username { get; set; }
    public string? AccessToken { get; set; }
    public List<string>? Role { get; set; }
    public List<string>? Permission { get; set; }
    public RefreshTokenResult? RefreshToken { get; set; }
    public List<Project> Projects { get; set; }
}

public class RefreshTokenResult
{
    public string? Token { get; set; }
    public DateTime? ExpiryDate { get; set; }
}

public class Project
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public string Route { get; set; }
}
