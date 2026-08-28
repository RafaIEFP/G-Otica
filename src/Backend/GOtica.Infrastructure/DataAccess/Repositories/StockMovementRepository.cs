using GOtica.Domain.Entities;
using GOtica.Domain.Repositories.StockMovement;

namespace GOtica.Infrastructure.DataAccess.Repositories;

internal sealed class StockMovementRepository(GOticaDbContext dbContext) : IStockMovementWriteOnlyRepository
{
    public async Task Add(StockMovement stockMovement)
    {
        await dbContext.StockMovements.AddAsync(stockMovement);
    }
}
