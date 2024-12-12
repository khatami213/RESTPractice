using System.ComponentModel.DataAnnotations;

namespace Core.ASP.Net.Infrastructure.JwtToken.Models;

public class RefreshTokenRequest
{
    [Required]
    public string RefreshToken { get; set; }
}
