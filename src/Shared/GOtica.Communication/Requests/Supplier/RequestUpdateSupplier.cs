namespace GOtica.Communication.Requests.Supplier;

public record RequestUpdateSupplier
{
    public string Name { get; init; } = string.Empty;
    public string? PhoneNumber { get; init; }
    public string? Email { get; init; }
}
