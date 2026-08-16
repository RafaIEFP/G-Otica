namespace GOtica.Communication.Requests;

public record RequestOpticalStore
{
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public string TaxNumber { get; init; } = string.Empty;
}
