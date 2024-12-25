using Core.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Front.Domain.Models.Auth;

public class LoginRequest
{
    [Required(ErrorMessage ="نام کاربری الزامی است")]
    public string Username { get; set; }

    [Required(ErrorMessage = "رمز عبور الزامی است")]
    public string Password { get; set; }
}
