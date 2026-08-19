using GOtica.Domain.Entities;
using GOtica.Domain.Repositories.Refresh;
using Microsoft.EntityFrameworkCore;

namespace GOtica.Infrastructure.DataAccess.Repositories;

internal class RefreshTokenRepository(GOticaDbContext dbContext) : IRefreshTokenReadOnlyRepository, IRefreshTokenWriteOnlyRepository
{
    public async Task Add(RefreshToken refreshToken)
    {
        await dbContext.RefreshTokens.Where(rt => rt.UserId == refreshToken.UserId).ExecuteDeleteAsync();
        await dbContext.RefreshTokens.AddAsync(refreshToken);
    }

    public async Task DeleteUserRefresh(Guid userId)
        => await dbContext.RefreshTokens.Where(rt => rt.UserId == userId).ExecuteDeleteAsync();

    public async Task<RefreshToken?> Get(string token)
        => await dbContext.RefreshTokens
        .AsNoTracking()
        .Include(rt => rt.User)
        .FirstOrDefaultAsync(rt => rt.Token.Equals(token));

    public Task<bool> HasRefresTokenAssociated(Guid userId, Guid accessTokenIdentifier)
        => dbContext.RefreshTokens.AnyAsync(rt => rt.UserId == userId && rt.AccessTokenId == accessTokenIdentifier);
}
