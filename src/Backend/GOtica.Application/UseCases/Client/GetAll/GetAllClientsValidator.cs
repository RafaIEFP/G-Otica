using FluentValidation;
using GOtica.Communication.Requests.Client;

namespace GOtica.Application.UseCases.Client.GetAll;

public class GetAllClientsValidator : AbstractValidator<RequestGetAllClients>
{
    public GetAllClientsValidator()
    {
        RuleFor(request => request.Page).GreaterThan(0);
        RuleFor(request => request.PageSize).InclusiveBetween(1, 100);
    }
}
