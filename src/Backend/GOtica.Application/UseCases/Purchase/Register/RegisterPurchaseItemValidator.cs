using FluentValidation;
using GOtica.Communication.Requests.PurchaseItem;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.Purchase.Register;

internal class RegisterPurchaseItemValidator : AbstractValidator<RequestRegisterPurchaseItem>
{
    public RegisterPurchaseItemValidator()
    {
        RuleFor(item => item.ProductId)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.PRODUCT_ID_EMPTY);

        RuleFor(item => item.Quantity)
            .GreaterThan(0)
            .WithMessage(ResourceMessagesException.QUANTITY_INVALID);

        RuleFor(item => item.UnitPrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage(ResourceMessagesException.UNIT_PRICE_NEGATIVE);
    }
}
