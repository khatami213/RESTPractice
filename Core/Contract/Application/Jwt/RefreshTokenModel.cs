namespace Core.Contract.Application.Jwt;

public class RefreshTokenModel
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string UserName { get; set; }
    public string Token { get; set; }
    public DateTime ExpiryDate { get; set; }
    public DateTime CreatedOn { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? RevokedOn { get; set; }
    public string? RevokedBy { get; set; }
    public string? ReplaceByToken { get; set; }
    public bool? IsExpired { get; set; }
    public bool? IsActive { get; set; }
    public bool IsUsed { get; set; }
    public bool IsRevoked { get; set; }
}
