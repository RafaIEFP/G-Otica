using FluentValidation;
using GOtica.Communication.Requests.Product;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.Product.AdjustStock;

public class AdjustProductStockValidator : AbstractValidator<RequestAdjustProductStock>
{
    public AdjustProductStockValidator()
    {
        RuleFor(request => request.Type).IsInEnum().WithMessage(ResourceMessagesException.STOCK_ADJUSTMENT_TYPE_INVALID);
        RuleFor(request => request.Quantity).GreaterThan(0).WithMessage(ResourceMessagesException.STOCK_ADJUSTMENT_QUANTITY_INVALID);

        RuleFor(request => request.Reason)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.STOCK_ADJUSTMENT_REASON_EMPTY)
            .MaximumLength(500);
    }
}
