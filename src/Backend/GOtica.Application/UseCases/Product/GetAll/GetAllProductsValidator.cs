using FluentValidation;
using GOtica.Communication.Requests.Product;

namespace GOtica.Application.UseCases.Product.GetAll;

public class GetAllProductsValidator : AbstractValidator<RequestGetAllProducts>
{
    public GetAllProductsValidator()
    {
        RuleFor(request => request.Page).GreaterThan(0);

        RuleFor(request => request.PageSize).InclusiveBetween(1, 100);
    }
}
