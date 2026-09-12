using GOtica.Domain.Enums;

namespace GOtica.Domain.Repositories.Payment;

public interface IPaymentUpdateOnlyRepository
{
    Task<bool> TryReceive(
        Guid paymentId,
        Guid saleId,
        Guid opticalStoreId,
        PaymentMethod paymentMethod,
        Guid receivedByUserId,
        DateTime receivedAt);

    Task CancelBySale(Guid saleId, Guid opticalStoreId);
}
