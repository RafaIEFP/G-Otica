namespace GOtica.Communication.Response.Client;

public record ResponseGetClient
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public string? Email { get; init; }
    public DateOnly? DateOfBirth { get; init; }
    public bool IsActive { get; init; }
}
