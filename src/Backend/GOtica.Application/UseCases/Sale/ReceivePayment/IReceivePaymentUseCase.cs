using GOtica.Communication.Requests.Payment;

namespace GOtica.Application.UseCases.Sale.ReceivePayment;

public interface IReceivePaymentUseCase
{
    Task Execute(Guid opticalStoreId, Guid saleId, Guid paymentId, RequestReceivePayment request);
}
