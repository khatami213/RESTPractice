using Authorization.Domain.Models.WriteModels.Users;

namespace Authorization.Domain.Models.ReadModels.Users;

public class UserRoleReadModel : BaseEntity
{
    public long UserId { get; set; }
    public long RoleId { get; set; }
    public UserReadModel User { get; set; }
    public RoleReadModel Role { get; set; }
}
