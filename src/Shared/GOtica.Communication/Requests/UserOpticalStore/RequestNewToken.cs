namespace GOtica.Communication.Requests.UserOpticalStore;

public record RequestNewToken
{
    public string RefreshToken { get; init; } = string.Empty;
    public string AccessToken { get; init; } = string.Empty;
}
