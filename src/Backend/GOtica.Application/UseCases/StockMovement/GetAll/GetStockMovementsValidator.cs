using FluentValidation;
using GOtica.Communication.Requests.StockMovement;

namespace GOtica.Application.UseCases.StockMovement.GetAll;

public class GetStockMovementsValidator : AbstractValidator<RequestGetStockMovements>
{
    public GetStockMovementsValidator()
    {
        RuleFor(request => request.Page).GreaterThan(0);

        RuleFor(request => request.PageSize).InclusiveBetween(1, 100);
    }
}
