using GOtica.Domain.Entities;
using GOtica.Domain.Repositories.Refresh;
using Microsoft.EntityFrameworkCore;

namespace GOtica.Infrastructure.DataAccess.Repositories;

internal class RefreshTokenRepository : IRefreshTokenReadOnlyRepository, IRefreshTokenWriteOnlyRepository
{
    private readonly GOticaDbContext _dbContext;
    public RefreshTokenRepository(GOticaDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(RefreshToken refreshToken)
    {
        await _dbContext.RefreshTokens.Where(rt => rt.UserId == refreshToken.UserId).ExecuteDeleteAsync();
        await _dbContext.RefreshTokens.AddAsync(refreshToken);
    }

    public async Task<RefreshToken?> Get(string token)
        => await _dbContext.RefreshTokens
        .AsNoTracking()
        .Include(rt => rt.User)
        .FirstOrDefaultAsync(rt => rt.Token.Equals(token));
}
