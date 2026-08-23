namespace GOtica.Domain.Repositories.Invite;

public interface IInviteUpdateOnlyRepository
{
    Task UpdateStatusToAccepted(Guid inviteId);
}
