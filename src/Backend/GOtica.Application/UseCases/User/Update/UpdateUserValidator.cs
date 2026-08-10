using FluentValidation;
using GOtica.Communication.Requests;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.User.Update;

public class UpdateUserValidator : AbstractValidator<RequestUpdateUser>
{
    public UpdateUserValidator()
    {
        RuleFor(request => request.Name).NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY);
        RuleFor(request => request.Email)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.EMAIL_EMPTY)
            .EmailAddress()
            .When(request => !string.IsNullOrWhiteSpace(request.Email), ApplyConditionTo.CurrentValidator)
            .WithMessage(ResourceMessagesException.EMAIL_INVALID);
    }
}
