using FluentValidation;
using GOtica.Communication.Response.Product;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.Product.Update;

internal class UpdateProductValidator : AbstractValidator<RequestUpdateProduct>
{
    public UpdateProductValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.NAME_EMPTY)
            .MaximumLength(255);

        RuleFor(request => request.ProductType)
            .IsInEnum()
            .WithMessage(ResourceMessagesException.PRODUCT_TYPE_INVALID);

        RuleFor(request => request.ProductCode)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.PRODUCT_CODE_EMPTY)
            .MaximumLength(100);

        RuleFor(request => request.BasePrice)
            .GreaterThan(0)
            .WithMessage(ResourceMessagesException.BASE_PRICE_INVALID);
    }
}
