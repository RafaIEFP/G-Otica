using FluentValidation;
using GOtica.Communication.Requests.Purchase;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.Purchase.GetAll;

internal class GetAllPurchasesValidator : AbstractValidator<RequestGetAllPurchases>
{
    public GetAllPurchasesValidator()
    {
        RuleFor(request => request.Page).GreaterThan(0).WithMessage(ResourceMessagesException.PAGE_INVALID);
        RuleFor(request => request.PageSize).InclusiveBetween(1, 100).WithMessage(ResourceMessagesException.PAGE_SIZE_INVALID);
    }
}
