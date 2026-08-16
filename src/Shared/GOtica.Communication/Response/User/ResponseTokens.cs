namespace GOtica.Communication.Response.User;

public record ResponseTokens
{
    public string AccessToken { get; init; } = string.Empty;
    public string RefreshToken { get; init; } = string.Empty;
}
