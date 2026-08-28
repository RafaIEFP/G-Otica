using FluentValidation;
using GOtica.Communication.Requests.Supplier;

namespace GOtica.Application.UseCases.Supplier.GetAll;

internal class GetAllSuppliersValidator : AbstractValidator<RequestGetAllSuppliers>
{
    public GetAllSuppliersValidator()
    {
        RuleFor(request => request.Page).GreaterThan(0);
        RuleFor(request => request.PageSize).InclusiveBetween(1, 100);
    }
}
