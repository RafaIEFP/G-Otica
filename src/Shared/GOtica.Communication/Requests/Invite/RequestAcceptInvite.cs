namespace GOtica.Communication.Requests.Invite;

public record RequestAcceptInvite
{
    public string Token { get; init; } = string.Empty;
}
