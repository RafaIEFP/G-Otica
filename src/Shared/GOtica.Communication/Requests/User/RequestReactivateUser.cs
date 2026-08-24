namespace GOtica.Communication.Requests.User;

public record RequestReactivateUser
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}
