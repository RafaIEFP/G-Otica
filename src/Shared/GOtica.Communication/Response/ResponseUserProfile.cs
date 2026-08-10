namespace GOtica.Communication.Response;

public record ResponseUserProfile
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public IReadOnlyCollection<ResponseOpticalStoreProfile> OpticalStores { get; init; } = [];
}
