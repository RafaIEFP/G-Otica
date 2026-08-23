namespace GOtica.Communication.Response.UserOpticalStore;

public record ResponseGetAllOpticalStoreUser
{
    public Guid UserId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public DateOnly EntranceDate { get; init; }
}
