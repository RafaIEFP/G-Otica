using GOtica.Domain.Dtos;
using GOtica.Domain.Entities;
using GOtica.Domain.Repositories;
using GOtica.Domain.Repositories.Product;
using Microsoft.EntityFrameworkCore;

namespace GOtica.Infrastructure.DataAccess.Repositories;

internal sealed class ProductRepository(GOticaDbContext dbContext) : IProductUpdateOnlyRepository, IProductReadOnlyRepository, IProductWriteOnlyRepository
{
    public async Task Add(Product product)
    {
        await dbContext.Products.AddAsync(product);
    }

    public async Task<bool> Deactivate(Guid productId, Guid opticalStoreId)
    {
        var affectedRows = await dbContext.Products
            .Where(product =>
                product.Id == productId &&
                product.OpticalStoreId == opticalStoreId &&
                product.IsActive)
            .ExecuteUpdateAsync(setter =>
                setter.SetProperty(
                    product => product.IsActive, false));

        return affectedRows > 0;
    }

    public async Task<Product?> GetActiveInOpticalStore(Guid productId, Guid opticalStoreId)
    {
        return await dbContext.Products
            .FirstOrDefaultAsync(product =>
                product.Id == productId &&
                product.OpticalStoreId == opticalStoreId &&
                product.IsActive);
    }

    public async Task<PagedResult<ProductDto>> GetAll(Guid opticalStoreId, int page, int pageSize, bool? isActive)
    {
        var query = dbContext.Products
            .AsNoTracking()
            .Where(product =>product.OpticalStoreId == opticalStoreId);

        if (isActive.HasValue)
        {
            query = query.Where(product =>
                product.IsActive == isActive.Value);
        }

        var totalCount = await query.CountAsync();

        var products = await query
            .OrderBy(product => product.Name)
            .ThenBy(product => product.Id)
            .Paged(page, pageSize)
            .Select(product => new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                ProductType = product.ProductType,
                ProductCode = product.ProductCode,
                BasePrice = product.BasePrice,
                StockQuantity = product.StockQuantity,
                IsActive = product.IsActive
            })
            .ToListAsync();

        return new PagedResult<ProductDto>
        {
            Items = products,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<Product?> GetById(Guid productId, Guid opticalStoreId)
    {
        return await dbContext.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == productId && p.OpticalStoreId == opticalStoreId);
    }

    public async Task<bool> ProductAlreadyAtOpticalStore(string productCode, Guid opticalStoreId)
    {
        return await dbContext.Products.AnyAsync(
            p => p.OpticalStoreId == opticalStoreId && 
            p.ProductCode == productCode);
    }

    public async Task<bool> ProductCodeAlreadyAtOpticalStore(string productCode, Guid opticalStoreId, Guid exceptProductId)
    {
        return await dbContext.Products.AnyAsync(product =>
            product.OpticalStoreId == opticalStoreId &&
            product.ProductCode == productCode &&
            product.Id != exceptProductId);
    }

    public async Task<bool> Reactivate(Guid productId, Guid opticalStoreId)
    {
        var affectedRows = await dbContext.Products
            .Where(product =>
                product.Id == productId &&
                product.OpticalStoreId == opticalStoreId &&
                !product.IsActive)
            .ExecuteUpdateAsync(setter =>
                setter.SetProperty(
                    product => product.IsActive, true));

        return affectedRows > 0;
    }
}
