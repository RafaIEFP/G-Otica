namespace GOtica.Communication.Response.Supplier;

public record ResponseSupplier
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? PhoneNumber { get; init; }
    public string? Email { get; init; }
    public bool IsActive { get; init; }
}
