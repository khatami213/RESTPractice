namespace Authorization.Domain.Models.ReadModels.Users;

public class RolePermissionReadModel
{
    public long Id { get; set; }
    public long RoleId { get; set; }
    public RoleReadModel Role { get; set; }
    public long PermissionId { get; set; }
    public PermissionReadModel Permission { get; set; }
}
