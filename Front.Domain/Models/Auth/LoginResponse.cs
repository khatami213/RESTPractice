using Front.Domain.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Front.Domain.Models.Auth;

public class LoginResponse
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
