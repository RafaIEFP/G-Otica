using GOtica.Domain.Entities;
using GOtica.Domain.Enums;
using GOtica.Domain.Repositories.Invite;
using Microsoft.EntityFrameworkCore;

namespace GOtica.Infrastructure.DataAccess.Repositories;

internal sealed class InviteRepository(GOticaDbContext dbContext) : IInviteReadOnlyRepository, IInviteWriteOnlyRepository, IInviteUpdateOnlyRepository
{
    public async Task Add(Invite invite)
    {
        await dbContext.Invites.AddAsync(invite);
    }

    public async Task<bool> ExistsPendingInvite(string guestEmail, Guid opticalStoreId)
    {
        return await dbContext.Invites.AnyAsync(
            i => i.GuestEmail == guestEmail &&
            i.OpticalStoreId == opticalStoreId &&
            i.Status == InviteStatus.Pending &&
            i.ExpiresAt > DateTime.UtcNow
        );
    }

    public async Task<Invite?> GetValidInviteByTokenHash(string tokenHash)
    {
        return await dbContext.Invites.AsNoTracking().FirstOrDefaultAsync(i => 
            i.TokenHash == tokenHash && 
            i.ExpiresAt > DateTime.UtcNow && 
            i.Status == InviteStatus.Pending);
    }

    public async Task UpdateStatusToAccepted(Guid inviteId)
    {
        await dbContext.Invites
            .Where(i => i.Id == inviteId)
            .ExecuteUpdateAsync(setter => setter.SetProperty(i => i.Status, InviteStatus.Accepted));
    }
}
