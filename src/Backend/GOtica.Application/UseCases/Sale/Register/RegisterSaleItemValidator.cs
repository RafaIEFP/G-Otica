using FluentValidation;
using GOtica.Communication.Requests.Sale;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.Sale.Register;

internal class RegisterSaleItemValidator : AbstractValidator<RequestRegisterSaleItem>
{
    public RegisterSaleItemValidator()
    {
        RuleFor(r => r.ProductId).NotEmpty().WithMessage(ResourceMessagesException.PRODUCT_ID_EMPTY);

        RuleFor(r => r.Quantity)
            .GreaterThan(0)
            .WithMessage(ResourceMessagesException.QUANTITY_INVALID);


        RuleFor(r => r.Quantity)
            .Equal(1)
            .When(item =>
                item.ItemLens is not null &&
                item.Quantity > 0)
            .WithMessage(ResourceMessagesException.LENS_ITEM_QUANTITY_MUST_BE_ONE);

        RuleFor(r => r.DiscountAmount).GreaterThanOrEqualTo(0).WithMessage(ResourceMessagesException.DISCOUNT_AMOUNT_INVALID);
        RuleFor(r => r.Notes).MaximumLength(500).WithMessage(ResourceMessagesException.NOTES_SALE_MAX_LENGTH);

        RuleFor(r => r.ItemLens)
            .SetValidator(new RegisterSaleItemLensValidator()!)
            .When(item => item.ItemLens is not null);
    }
}
