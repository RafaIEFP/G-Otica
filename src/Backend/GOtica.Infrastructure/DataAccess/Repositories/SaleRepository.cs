using GOtica.Domain.Entities;
using GOtica.Domain.Repositories.Sale;

namespace GOtica.Infrastructure.DataAccess.Repositories;

internal sealed class SaleRepository(GOticaDbContext dbContext) : ISaleWriteOnlyRepository
{
    public async Task Add(Sale sale)
    {
        await dbContext.Sales.AddAsync(sale);
    }
}
