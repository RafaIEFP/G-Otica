namespace GOtica.Communication.Response.UserOpticalStore;

public record ResponseValidateInvite
{
    public bool RequiresRegistration { get; init; }
    public bool RequiresReactivation { get; init; }
}
