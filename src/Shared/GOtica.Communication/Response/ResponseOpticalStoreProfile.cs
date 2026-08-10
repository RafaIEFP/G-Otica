namespace GOtica.Communication.Response;

public record ResponseOpticalStoreProfile
{
    public long Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
}
