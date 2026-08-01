namespace GOtica.Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Token { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid AccessTokenId { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = default!;
}
