using GOtica.Domain.Enums;

namespace GOtica.Domain.Entities;

public class Invite
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string TokenHash { get; set; } = string.Empty;
    public InviteStatus Status { get; set; } = InviteStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAt { get; set; }

    public Guid OpticalStoreId { get; set; }
    public OpticalStore OpticalStore { get; set; } = default!;
    public Guid InvitedByUserId { get; set; }
    public User InvitedByUser { get; set; } = default!;
}
