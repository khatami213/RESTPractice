using Authorization.Query.Contract.Dtos.Roles;

namespace Authorization.Query.Contract.Dtos.Users;

public class UserDto
{
    public string Username { get; set; }
    public string? Email { get; set; }
    public List<RoleDto>? Roles { get; set; }
}
