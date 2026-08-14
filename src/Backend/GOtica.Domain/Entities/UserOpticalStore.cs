namespace GOtica.Domain.Entities;

public class UserOpticalStore
{
    public DateOnly EntranceDate { get; set; }
    public string Role { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public Guid OpticalStoreId { get; set; }
    public OpticalStore OpticalStore { get; set; } = default!;
}
