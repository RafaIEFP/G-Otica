using GOtica.Domain.Entities;
using GOtica.Domain.Repositories.UserOpticalStore;
using Microsoft.EntityFrameworkCore;

namespace GOtica.Infrastructure.DataAccess.Repositories;

internal sealed class UserOpticalStoreRepository : IUserOpticalStoreReadOnlyRepository
{
    private readonly GOticaDbContext _dbContext;
    public UserOpticalStoreRepository(GOticaDbContext dbContext)
        => _dbContext = dbContext;

    public async Task<IReadOnlyCollection<UserOpticalStore>> GetUserOpticalStores(Guid userId)
    {
        return await _dbContext.UserOpticalStores
            .AsNoTracking()
            .Include(uos => uos.OpticalStore)
            .Where(uos => uos.UserId == userId)
            .ToListAsync();
    }
}
