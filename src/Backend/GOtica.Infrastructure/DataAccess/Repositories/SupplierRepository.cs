using GOtica.Domain.Entities;
using GOtica.Domain.Repositories.Supplier;
using Microsoft.EntityFrameworkCore;

namespace GOtica.Infrastructure.DataAccess.Repositories;

internal sealed class SupplierRepository(GOticaDbContext dbContext) : ISupplierWriteOnlyRepository, ISupplierReadOnlyRepository
{
    public async Task Add(Supplier supplier)
    {
        await dbContext.Suppliers.AddAsync(supplier);
    }

    public async Task<Supplier?> GetById(Guid supplierId, Guid opticalStoreId)
    {
        return await dbContext.Suppliers
        .AsNoTracking()
        .FirstOrDefaultAsync(supplier => supplier.Id == supplierId && supplier.OpticalStoreId == opticalStoreId);
    }
}
