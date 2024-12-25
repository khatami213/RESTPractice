namespace Authorization.Domain.Models.WriteModels.Users;

public class User : BaseEntity
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
    public string? Email { get; set; }
    public List<Role>? Roles { get; set; }
    public List<Project>? Projects { get; set; }
}
