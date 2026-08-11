using GOtica.Domain.Repositories.OpticalStore;
using Microsoft.EntityFrameworkCore;

namespace GOtica.Infrastructure.DataAccess.Repositories;

internal sealed class OpticalStoreRepository : IOpticalStoreReadOnlyRepository
{
    private readonly GOticaDbContext _dbContext;
    public OpticalStoreRepository(GOticaDbContext dbContext) => _dbContext = dbContext;

    public Task<bool> ExistsActiveOptical(long opticalId)
        => _dbContext.OpticalStores.AnyAsync(op => op.Id == opticalId && op.IsActive);
}
