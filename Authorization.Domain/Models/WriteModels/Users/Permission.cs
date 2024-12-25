namespace Authorization.Domain.Models.WriteModels.Users;

public class Permission
{
    public long Id { get; set; }
    public string Title { get; set; }
    public long? ParentId { get; set; }
    public List<Role> Roles { get; set; }
}
