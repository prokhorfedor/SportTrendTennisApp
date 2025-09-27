namespace Database.Entities;

public class RefreshToken
{
    public Guid RefreshTokenId { get; set; }
    public string Token { get; set; }
    public string JwtId { get; set; }
    public bool Used { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public Guid? UserId { get; set; }
}