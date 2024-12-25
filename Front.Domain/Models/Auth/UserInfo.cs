namespace Front.Domain.Models.Auth;

public class UserInfo
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public List<string>? Roles { get; set; }
    public List<string>? Permissions { get; set; }
    public List<string>? AllowedApplications { get; set; }
}
