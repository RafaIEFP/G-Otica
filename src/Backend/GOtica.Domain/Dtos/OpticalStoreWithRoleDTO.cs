namespace GOtica.Domain.Dtos;

public class OpticalStoreWithRoleDTO
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public string TaxNumber { get; init; } = string.Empty;
    public DateOnly EntranceDate { get; init; }
    public bool IsActive { get; init; }
    public string Role { get; init; } = string.Empty;
}
