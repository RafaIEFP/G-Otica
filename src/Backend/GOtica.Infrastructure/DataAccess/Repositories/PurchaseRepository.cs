using GOtica.Domain.Dtos;
using GOtica.Domain.Entities;
using GOtica.Domain.Repositories;
using GOtica.Domain.Repositories.Purchase;
using Microsoft.EntityFrameworkCore;

namespace GOtica.Infrastructure.DataAccess.Repositories;

internal sealed class PurchaseRepository(GOticaDbContext dbContext) : IPurchaseWriteOnlyRepository, IPurchaseReadOnlyRepository
{
    public async Task Add(Purchase purchase)
    {
        await dbContext.Purchases.AddAsync(purchase);
    }

    public async Task<PagedResult<PurchaseListDto>> GetAll(Guid opticalStoreId, int page, int pageSize)
    {
        var query = dbContext.Purchases
            .AsNoTracking()
            .Where(purchase => purchase.OpticalStoreId == opticalStoreId);

        var totalCount = await query.CountAsync();

        var purchases = await query
            .OrderByDescending(purchase => purchase.CreatedAt)
            .ThenByDescending(purchase => purchase.Id)
            .Paged(page, pageSize)
            .Select(purchase => new PurchaseListDto
            {
                Id = purchase.Id,
                CreatedAt = purchase.CreatedAt,
                TotalAmount = purchase.TotalAmount,
                SupplierId = purchase.SupplierId,
                SupplierName = purchase.Supplier.Name,
                UserId = purchase.UserId,
                UserName = purchase.User.Name
            })
            .ToListAsync();

        return new PagedResult<PurchaseListDto>
        {
            Items = purchases,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<PurchaseDto?> GetById(Guid purchaseId, Guid opticalStoreId)
    {
        return await dbContext.Purchases
            .AsNoTracking()
            .Where(purchase =>
                purchase.Id == purchaseId &&
                purchase.OpticalStoreId == opticalStoreId)
            .Select(purchase => new PurchaseDto
            {
                Id = purchase.Id,
                CreatedAt = purchase.CreatedAt,
                TotalAmount = purchase.TotalAmount,
                SupplierId = purchase.SupplierId,
                SupplierName = purchase.Supplier.Name,
                UserId = purchase.UserId,
                UserName = purchase.User.Name,

                Items = purchase.Items
                    .OrderBy(item => item.Id)
                    .Select(item => new PurchaseItemDto
                    {
                        Id = item.Id,
                        ProductId = item.ProductId,
                        ProductName = item.Product.Name,
                        ProductCode = item.Product.ProductCode,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        TotalAmount = item.TotalAmount
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }
}
