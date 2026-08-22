namespace GOtica.Domain.Repositories.Invite;

public interface IInviteReadOnlyRepository
{
    Task<bool> ExistsPendingInvite(string guestEmail, Guid opticalStoreId);
    Task<Entities.Invite?> GetValidInviteByTokenHash(string tokenHash);
}
