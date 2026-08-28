using GOtica.Domain.Entities;
using GOtica.Domain.Repositories.Supplier;

namespace GOtica.Infrastructure.DataAccess.Repositories;

internal sealed class SupplierRepository(GOticaDbContext dbContext) : ISupplierWriteOnlyRepository
{
    public async Task Add(Supplier supplier)
    {
        await dbContext.Suppliers.AddAsync(supplier);
    }
}
