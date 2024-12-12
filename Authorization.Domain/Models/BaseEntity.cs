namespace Authorization.Domain.Models;

public abstract class BaseEntity
{
    public long Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? LastUpdatedOn { get; set; }
    public string? LastUpdatedBy { get; set; }
}
