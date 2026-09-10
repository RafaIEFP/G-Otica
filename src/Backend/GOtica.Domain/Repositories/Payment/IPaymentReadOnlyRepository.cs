namespace GOtica.Domain.Repositories.Payment;

public interface IPaymentReadOnlyRepository
{
    Task<bool> Exist(Guid paymentId, Guid saleId, Guid opticalStoreId);
}
