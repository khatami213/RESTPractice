namespace Authorization.Domain.Models.ReadModels.Users;

public class PermissionReadModel
{
    public long Id { get; set; }
    public string Title { get; set; }
    public long? ParentId { get; set; }
    public List<RoleReadModel> Roles { get; set; }
}
