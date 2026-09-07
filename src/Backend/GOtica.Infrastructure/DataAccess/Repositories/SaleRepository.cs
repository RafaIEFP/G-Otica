using GOtica.Domain.Dtos;
using GOtica.Domain.Entities;
using GOtica.Domain.Enums;
using GOtica.Domain.Repositories;
using GOtica.Domain.Repositories.Sale;
using Microsoft.EntityFrameworkCore;

namespace GOtica.Infrastructure.DataAccess.Repositories;

internal sealed class SaleRepository(GOticaDbContext dbContext) : ISaleWriteOnlyRepository, ISaleReadOnlyRepository, ISaleUpdateOnlyRepository
{
    public async Task Add(Sale sale)
    {
        await dbContext.Sales.AddAsync(sale);
    }

    public async Task<bool> Exist(Guid saleId, Guid opticalStoreId)
    {
        return await dbContext.Sales
            .AsNoTracking()
            .AnyAsync(sale =>
                sale.Id == saleId &&
                sale.OpticalStoreId == opticalStoreId);
    }

    public async Task<PagedResult<SaleListDto>> GetAll(Guid opticalStoreId, int page, int pageSize, SaleStatus? status)
    {
        var query = dbContext.Sales.AsNoTracking().Where(sale => sale.OpticalStoreId == opticalStoreId);

        if (status.HasValue)
            query = query.Where(sale => sale.Status == status.Value);

        var totalCount = await query.CountAsync();

        var sales = await query
            .OrderByDescending(sale => sale.CreatedAt)
            .ThenByDescending(sale => sale.Id)
            .Paged(page, pageSize)
            .Select(sale => new SaleListDto
            {
                Id = sale.Id,
                CreatedAt = sale.CreatedAt,
                Status = sale.Status,
                TotalAmount = sale.TotalAmount,

                ReceivedAmount = sale.Payments
                    .Where(payment => payment.Status == PaymentStatus.Received)
                    .Sum(payment => payment.Amount),

                ClientId = sale.ClientId,
                ClientName = sale.Client.Name
            })
            .ToListAsync();

        return new PagedResult<SaleListDto>
        {
            Items = sales,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<SaleDto?> GetById(Guid saleId, Guid opticalStoreId)
    {
        return await dbContext.Sales
            .AsNoTracking()
            .Where(sale =>
                sale.Id == saleId &&
                sale.OpticalStoreId == opticalStoreId)
            .Select(sale => new SaleDto
            {
                Id = sale.Id,
                CreatedAt = sale.CreatedAt,
                Status = sale.Status,
                TotalAmount = sale.TotalAmount,
                ClientId = sale.ClientId,
                ClientName = sale.Client.Name,
                UserId = sale.UserId,
                UserName = sale.User.Name,
                PrescriptionId = sale.PrescriptionId,

                Items = sale.Items
                    .OrderBy(item => item.Id)
                    .Select(item => new SaleItemDto
                    {
                        Id = item.Id,
                        ProductId = item.ProductId,
                        ProductName = item.Product.Name,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        DiscountAmount = item.DiscountAmount,
                        TotalAmount = item.TotalAmount,
                        Notes = item.Notes
                    })
                    .ToList(),

                Payments = sale.Payments
                    .OrderBy(payment => payment.Id)
                    .Select(payment => new PaymentDto
                    {
                        Id = payment.Id,
                        Amount = payment.Amount,
                        Status = payment.Status,
                        PaymentMethod = payment.PaymentMethod,
                        ReceivedAt = payment.ReceivedAt,
                        ReceivedByUserId = payment.ReceivedByUserId,
                        ReceivedByUserName =
                            payment.ReceivedByUser != null
                                ? payment.ReceivedByUser.Name
                                : null
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> TryUpdateStatus(Guid saleId, Guid opticalStoreId, SaleStatus expectedStatus, SaleStatus newStatus)
    {
        var affectedRows = await dbContext.Sales
            .Where(sale =>
                sale.Id == saleId &&
                sale.OpticalStoreId == opticalStoreId &&
                sale.Status == expectedStatus)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(
                    sale => sale.Status,
                    newStatus));

        return affectedRows > 0;
    }
}
