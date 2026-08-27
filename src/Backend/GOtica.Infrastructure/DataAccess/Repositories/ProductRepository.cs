using GOtica.Domain.Entities;
using GOtica.Domain.Repositories.Product;
using Microsoft.EntityFrameworkCore;

namespace GOtica.Infrastructure.DataAccess.Repositories;

internal sealed class ProductRepository(GOticaDbContext dbContext) : IProductUpdateOnlyRepository, IProductReadOnlyRepository, IProductWriteOnlyRepository
{
    public async Task Add(Product product)
    {
        await dbContext.Products.AddAsync(product);
    }

    public async Task<bool> ProductAlreadyAtOpticalStore(string productCode, Guid opticalStoreId)
    {
        return await dbContext.Products.AnyAsync(
            p => p.OpticalStoreId == opticalStoreId && 
            p.ProductCode == productCode);
    }
}
