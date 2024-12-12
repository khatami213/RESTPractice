namespace Authorization.Domain.Models.ReadModels.Users;

public class UserReadModel : BaseEntity
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
    public string? Email { get; set; }
    public List<RoleReadModel>? Roles { get; set; }
}
