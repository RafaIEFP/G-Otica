using FluentValidation;
using GOtica.Communication.Requests.Treatment;

namespace GOtica.Application.UseCases.Treatment.GetAll;

internal class GetAllTreatmentsValidator : AbstractValidator<RequestGetAllTreatments>
{
    public GetAllTreatmentsValidator()
    {
        RuleFor(request => request.Page)
            .GreaterThan(0);

        RuleFor(request => request.PageSize)
            .InclusiveBetween(1, 100);
    }
}
