using FluentValidation;
using GOtica.Communication.Requests.Treatment;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.Treatment.Register;

internal class RegisterTreatmentValidator : AbstractValidator<RequestRegisterTreatment>
{
    public RegisterTreatmentValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.NAME_EMPTY)
            .MaximumLength(255);

        RuleFor(request => request.BasePrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage(ResourceMessagesException.TREATMENT_BASE_PRICE_INVALID);
    }
}
