namespace GOtica.Communication.Requests;

public record RequestReactivateUser
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}
