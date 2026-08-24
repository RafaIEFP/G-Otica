namespace GOtica.Communication.Requests.UserOpticalStore;

public record RequestChangeRole
{
    public string Role { get; init; } = string.Empty;
}
