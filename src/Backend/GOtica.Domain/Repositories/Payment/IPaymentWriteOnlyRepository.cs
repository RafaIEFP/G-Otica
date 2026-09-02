namespace GOtica.Domain.Repositories.Payment;

public interface IPaymentWriteOnlyRepository
{
    Task AddRange(IReadOnlyCollection<Entities.Payment> payments);
}
