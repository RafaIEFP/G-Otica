using FluentValidation;
using GOtica.Communication.Response.Sale;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.Sale.GetAll;

internal class GetAllSalesValidator : AbstractValidator<RequestGetAllSales>
{
    public GetAllSalesValidator()
    {
        RuleFor(request => request.Page).GreaterThan(0).WithMessage(ResourceMessagesException.PAGE_INVALID);

        RuleFor(request => request.PageSize).GreaterThan(0).WithMessage(ResourceMessagesException.PAGE_SIZE_INVALID);

        RuleFor(request => request.Status)
            .IsInEnum()
            .When(request => request.Status.HasValue)
            .WithMessage(ResourceMessagesException.SALE_STATUS_INVALID);
    }
}
