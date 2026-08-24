namespace GOtica.Communication.Requests.Invite;

public record RequestInvite
{
    public string GuestEmail { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
}
