using GOtica.Domain.Dtos;
using GOtica.Domain.Entities;
using GOtica.Domain.Repositories.Sale;
using Microsoft.EntityFrameworkCore;

namespace GOtica.Infrastructure.DataAccess.Repositories;

internal sealed class SaleRepository(GOticaDbContext dbContext) : ISaleWriteOnlyRepository, ISaleReadOnlyRepository
{
    public async Task Add(Sale sale)
    {
        await dbContext.Sales.AddAsync(sale);
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
}
