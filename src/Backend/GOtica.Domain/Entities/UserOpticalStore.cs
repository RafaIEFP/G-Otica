namespace GOtica.Domain.Entities;

public class UserOpticalStore
{
    public DateOnly EntranceDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public string Role { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = default!;

    public int OpticalStoreId { get; set; }
    public OpticalStore OpticalStore { get; set; } = default!;
}
