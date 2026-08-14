using GOtica.Domain.Entities;
using GOtica.Domain.Repositories.OpticalStore;
using Microsoft.EntityFrameworkCore;

namespace GOtica.Infrastructure.DataAccess.Repositories;

internal sealed class OpticalStoreRepository : IOpticalStoreReadOnlyRepository, IOpticalStoreWriteOnlyRepository
{
    private readonly GOticaDbContext _dbContext;
    public OpticalStoreRepository(GOticaDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(OpticalStore opticalStore)
    {
        await _dbContext.OpticalStores.AddAsync(opticalStore);
    }

    public async Task<bool> ExistOpticalStoreRegistered(string taxNumber)
    {
        return await _dbContext.OpticalStores.AnyAsync(o => o.TaxNumber.Equals(taxNumber));
    }

    public Task<bool> ExistsActiveOptical(Guid opticalId)
        => _dbContext.OpticalStores.AnyAsync(op => op.Id == opticalId && op.IsActive);
}
