using GOtica.Domain.Entities;
using GOtica.Domain.Repositories.Purchase;

namespace GOtica.Infrastructure.DataAccess.Repositories;

internal sealed class PurchaseRepository(GOticaDbContext dbContext) : IPurchaseWriteOnlyRepository
{
    public async Task Add(Purchase purchase)
    {
        await dbContext.Purchases.AddAsync(purchase);
    }
}
