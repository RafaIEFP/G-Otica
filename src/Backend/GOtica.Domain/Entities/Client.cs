namespace GOtica.Domain.Entities;

public class Client
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Name { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Email { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public bool IsActive { get; set; } = true;

    public Guid OpticalStoreId { get; set; }
    public OpticalStore OpticalStore { get; set; } = default!;
}
