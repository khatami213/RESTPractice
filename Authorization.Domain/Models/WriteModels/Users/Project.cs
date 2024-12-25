namespace Authorization.Domain.Models.WriteModels.Users;

public class Project
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public string Route { get; set; }
    public List<User>? Users { get; set; }
}
