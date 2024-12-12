namespace Authorization.Application.ViewModels;

public class UserViewModel
{
    public long Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; }
}
