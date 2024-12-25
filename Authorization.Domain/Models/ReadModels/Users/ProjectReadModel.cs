namespace Authorization.Domain.Models.ReadModels.Users;

public class ProjectReadModel
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public string Route { get; set; }
    public List<UserReadModel>? Users { get; set; }
}
