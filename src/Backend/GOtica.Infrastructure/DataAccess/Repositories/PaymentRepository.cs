using GOtica.Domain.Entities;
using GOtica.Domain.Repositories.Payment;

namespace GOtica.Infrastructure.DataAccess.Repositories;

internal sealed class PaymentRepository(GOticaDbContext dbContext) : IPaymentWriteOnlyRepository
{
    public async Task AddRange(IReadOnlyCollection<Payment> payments)
    {
        await dbContext.Payments.AddRangeAsync(payments);
    }
}
