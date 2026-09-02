using FluentValidation;
using GOtica.Communication.Requests.Payment;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.Sale.Register;

internal class RegisterSalePaymentValidator : AbstractValidator<RequestRegisterSalePayment>
{
    public RegisterSalePaymentValidator()
    {
        RuleFor(payment => payment.Amount)
            .GreaterThan(0)
            .WithMessage(ResourceMessagesException.PAYMENT_AMOUNT_INVALID);

        RuleFor(payment => payment.PaymentMethod)
            .IsInEnum()
            .WithMessage(ResourceMessagesException.PAYMENT_METHOD_INVALID);

    }
}
