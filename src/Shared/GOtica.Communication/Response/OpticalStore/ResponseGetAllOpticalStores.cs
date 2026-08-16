namespace GOtica.Communication.Response.OpticalStore;

public record ResponseGetAllOpticalStores
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public DateOnly EntranceDate { get; init; }
    public bool IsActive { get; init; }
}
