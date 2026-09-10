using GOtica.Communication.Requests.Payment;
using GOtica.Domain.Repositories.Payment;
using GOtica.Domain.Services;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.Sale.ReceivePayment;

public class ReceivePaymentUseCase : IReceivePaymentUseCase
{
    private readonly IPaymentReadOnlyRepository _paymentReadOnlyRepository;
    private readonly IPaymentUpdateOnlyRepository _paymentUpdateOnlyRepository;
    private readonly ILoggedUser _loggedUser;

    public ReceivePaymentUseCase(
        IPaymentReadOnlyRepository paymentReadOnlyRepository, 
        IPaymentUpdateOnlyRepository paymentUpdateOnlyRepository,
        ILoggedUser loggedUser)
    {
        _paymentReadOnlyRepository = paymentReadOnlyRepository;
        _paymentUpdateOnlyRepository = paymentUpdateOnlyRepository;
        _loggedUser = loggedUser;
    }

    public async Task Execute(Guid opticalStoreId, Guid saleId, Guid paymentId, RequestReceivePayment request)
    {
        Validate(request);

        var loggedUser = await _loggedUser.Get();

        var received = await _paymentUpdateOnlyRepository.TryReceive(
            paymentId,
            saleId,
            opticalStoreId,
            (Domain.Enums.PaymentMethod)request.PaymentMethod!.Value,
            loggedUser.Id,
            DateTime.UtcNow
        );

        if (received)
            return;

        var paymentExists = await _paymentReadOnlyRepository.Exist(paymentId, saleId, opticalStoreId);

        if (!paymentExists)
            throw new NotFoundException(ResourceMessagesException.PAYMENT_NOT_FOUND);

        throw new ConflictException(ResourceMessagesException.PAYMENT_CANNOT_BE_RECEIVED);
    }

    private static void Validate(RequestReceivePayment request)
    {
        var result = new ReceivePaymentValidator().Validate(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }
}
