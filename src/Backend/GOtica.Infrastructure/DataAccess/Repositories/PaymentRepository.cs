using GOtica.Domain.Enums;
using GOtica.Domain.Repositories.Payment;
using Microsoft.EntityFrameworkCore;

namespace GOtica.Infrastructure.DataAccess.Repositories;

internal sealed class PaymentRepository(GOticaDbContext dbContext) : IPaymentReadOnlyRepository, IPaymentUpdateOnlyRepository
{
    public async Task<bool> Exist(Guid paymentId, Guid saleId, Guid opticalStoreId)
    {
        return await dbContext.Payments
            .AnyAsync(p => 
                p.Id == paymentId && 
                p.SaleId == saleId &&
                p.Sale.OpticalStoreId == opticalStoreId);
    }

    public async Task<bool> TryReceive(
        Guid paymentId, 
        Guid saleId, 
        Guid opticalStoreId, 
        PaymentMethod paymentMethod,
        Guid receivedByUserId,
        DateTime receivedAt)
    {
        var affectedRows = await dbContext.Payments
            .Where(p =>
                p.Id == paymentId &&
                p.SaleId == saleId &&
                p.Sale.OpticalStoreId == opticalStoreId &&
                p.Status == PaymentStatus.Pending &&
                (
                    p.Sale.Status == SaleStatus.Confirmed || 
                    p.Sale.Status == SaleStatus.Ready ||
                    p.Sale.Status == SaleStatus.InProduction
                ))
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(p => p.Status, PaymentStatus.Received)
                .SetProperty(p => p.PaymentMethod, paymentMethod)
                .SetProperty(p => p.ReceivedByUserId, receivedByUserId)
                .SetProperty(p => p.ReceivedAt, receivedAt));


        return affectedRows > 0;
    }
}
