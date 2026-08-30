using FluentValidation;
using GOtica.Communication.Requests.Purchase;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.Purchase.Register;

internal class RegisterPurchaseValidator : AbstractValidator<RequestRegisterPurchase>
{
    public RegisterPurchaseValidator()
    {
        RuleFor(request => request.SupplierId)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.SUPPLIER_ID_EMPTY);

        RuleFor(request => request.Items)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.PURCHASE_ITEMS_EMPTY);

        RuleForEach(request => request.Items).SetValidator(new RegisterPurchaseItemValidator());
    }
}
