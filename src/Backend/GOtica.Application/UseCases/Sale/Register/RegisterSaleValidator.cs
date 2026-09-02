using FluentValidation;
using GOtica.Communication.Requests.Sale;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.Sale.Register;

internal class RegisterSaleValidator : AbstractValidator<RequestRegisterSale>
{
    public RegisterSaleValidator()
    {
        RuleFor(r => r.ClientId).NotEmpty().WithMessage(ResourceMessagesException.CLIENT_ID_EMPTY);
        RuleFor(r => r.PrescriptionId)
            .Must(p => p is null || p != Guid.Empty)
            .WithMessage(ResourceMessagesException.PRESCRIPTION_ID_INVALID);

        RuleFor(r => r.Items)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.SALE_ITEMS_EMPTY);

        RuleForEach(r => r.Items).SetValidator(new RegisterSaleItemValidator());

        RuleFor(r => r.InitialPayment)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.INITIAL_PAYMENT_EMPTY)
            .SetValidator(new RegisterSalePaymentValidator());
    }
}
