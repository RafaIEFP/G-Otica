namespace GOtica.Communication.Requests;

public record RequestAcceptInvite
{
    public string Token { get; init; } = string.Empty;
}
