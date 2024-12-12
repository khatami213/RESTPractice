namespace Authorization.Domain.Models.ReadModels.Users;

public class RoleReadModel : BaseEntity
{
    public string Name { get; set; }
    public List<UserReadModel> Users { get; set; }
}
