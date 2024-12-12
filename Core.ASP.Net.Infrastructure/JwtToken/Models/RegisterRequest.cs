using System.ComponentModel.DataAnnotations;

namespace Core.ASP.Net.Infrastructure.JwtToken.Models;

public class RegisterRequest
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    [Compare("Password", ErrorMessage = "Passwords don't match!")]
    public string ConfirmPassword { get; set; }

    public string Email { get; set; }
}
