using GOtica.Domain.Entities;
using GOtica.Domain.Repositories.OpticalStore;
using Microsoft.EntityFrameworkCore;

namespace GOtica.Infrastructure.DataAccess.Repositories;

internal sealed class OpticalStoreRepository(GOticaDbContext dbContext) : IOpticalStoreReadOnlyRepository, IOpticalStoreWriteOnlyRepository, IOpticalStoreUpdateOnlyRepository
{
    public async Task Add(OpticalStore opticalStore)
    {
        await dbContext.OpticalStores.AddAsync(opticalStore);
    }

    public async Task DeactivateOpticalStore(Guid opticalStoreId)
    {
        await dbContext.OpticalStores
            .Where(o => o.Id == opticalStoreId && o.IsActive)
            .ExecuteUpdateAsync(
                setter => setter.SetProperty(o => o.IsActive, false)
                );
    }

    public async Task<bool> ExistOpticalStoreRegistered(string taxNumber)
    {
        return await dbContext.OpticalStores.AnyAsync(o => o.TaxNumber.Equals(taxNumber));
    }

    public Task<bool> ExistsActiveOptical(Guid opticalId)
        => dbContext.OpticalStores.AnyAsync(op => op.Id == opticalId && op.IsActive);

    public async Task<OpticalStore> GetById(Guid opticalStoreId)
    {
        return await dbContext.OpticalStores.SingleAsync(o => o.Id == opticalStoreId);
    }

    public void Update(OpticalStore opticalStore)
    {
        dbContext.OpticalStores.Update(opticalStore);
    }
}
