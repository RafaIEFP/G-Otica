namespace GOtica.Communication.Requests.Client;

public record RequestRegisterClient
{
    public string Name { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public string? Email { get; init; }
    public DateOnly? DateOfBirth { get; init; }
}
