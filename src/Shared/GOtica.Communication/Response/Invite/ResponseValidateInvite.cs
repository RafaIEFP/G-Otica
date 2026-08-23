namespace GOtica.Communication.Response.Invite;

public record ResponseValidateInvite
{
    public bool RequiresRegistration { get; init; }
    public bool RequiresReactivation { get; init; }
}
