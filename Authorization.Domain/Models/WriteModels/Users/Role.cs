namespace Authorization.Domain.Models.WriteModels.Users;

public class Role : BaseEntity
{
    public string Name { get; set; }
    public List<User> Users { get; set; }
}
