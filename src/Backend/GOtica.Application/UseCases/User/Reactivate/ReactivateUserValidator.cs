using FluentValidation;
using GOtica.Application.SharedValidators;
using GOtica.Communication.Requests.User;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.User.Reactivate;

public class ReactivateUserValidator : AbstractValidator<RequestReactivateUser>
{
    public ReactivateUserValidator()
    {
        RuleFor(request => request.Email)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.EMAIL_EMPTY)
            .EmailAddress()
            .When(request => !string.IsNullOrWhiteSpace(request.Email), ApplyConditionTo.CurrentValidator)
            .WithMessage(ResourceMessagesException.EMAIL_INVALID);

        RuleFor(request => request.Password).Password();
    }
}
