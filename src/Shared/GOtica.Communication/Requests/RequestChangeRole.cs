namespace GOtica.Communication.Requests;

public record RequestChangeRole
{
    public string Role { get; init; } = string.Empty;
}
