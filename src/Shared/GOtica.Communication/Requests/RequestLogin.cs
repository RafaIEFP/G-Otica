namespace GOtica.Communication.Requests;

public record RequestLogin
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}
