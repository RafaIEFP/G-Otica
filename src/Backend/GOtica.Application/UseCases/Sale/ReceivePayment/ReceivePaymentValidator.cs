using FluentValidation;
using GOtica.Communication.Requests.Payment;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.Sale.ReceivePayment;

internal class ReceivePaymentValidator : AbstractValidator<RequestReceivePayment>
{
    public ReceivePaymentValidator()
    {
        RuleFor(r => r.PaymentMethod).IsInEnum().WithMessage(ResourceMessagesException.PAYMENT_METHOD_INVALID);
    }
}
