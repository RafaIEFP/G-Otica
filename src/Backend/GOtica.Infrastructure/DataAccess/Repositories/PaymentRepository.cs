using GOtica.Domain.Repositories.Payment;

namespace GOtica.Infrastructure.DataAccess.Repositories;

internal sealed class PaymentRepository(GOticaDbContext dbContext) : IPaymentWriteOnlyRepository
{

}
