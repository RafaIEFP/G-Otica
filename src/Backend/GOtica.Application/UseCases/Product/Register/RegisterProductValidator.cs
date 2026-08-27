using FluentValidation;
using GOtica.Communication.Requests.Product;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.Product.Register;

public class RegisterProductValidator : AbstractValidator<RequestRegisterProduct>
{
    public RegisterProductValidator()
    {
        RuleFor(request => request.Name).NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY);
        RuleFor(request => request.ProductType).IsInEnum().WithMessage(ResourceMessagesException.PRODUCT_TYPE_INVALID);
        RuleFor(request => request.ProductCode).NotEmpty().WithMessage(ResourceMessagesException.PRODUCT_CODE_EMPTY);
        RuleFor(request => request.BasePrice).GreaterThan(0).WithMessage(ResourceMessagesException.BASE_PRICE_INVALID);
        RuleFor(request => request.StockQuantity).GreaterThanOrEqualTo(0).WithMessage(ResourceMessagesException.STOCK_INVALID);
    }
}
