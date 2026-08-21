using FluentValidation;
using GOtica.Communication.Requests;
using GOtica.Domain;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.UserOpticalStore.Invite;

internal class CreateInviteValidator : AbstractValidator<RequestInvite>
{
    private static readonly string[] AllowedRoles =
    [
        Roles.MANAGER,
        Roles.SALESPERSON
    ];

    public CreateInviteValidator()
    {
        RuleFor(r => r.GuestEmail)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.EMAIL_EMPTY)
            .EmailAddress()
            .When(request => !string.IsNullOrWhiteSpace(request.GuestEmail), ApplyConditionTo.CurrentValidator)
            .WithMessage(ResourceMessagesException.EMAIL_INVALID);

        RuleFor(r => r.Role).Must(role => AllowedRoles.Contains(role)).WithMessage(ResourceMessagesException.ROLE_INVALID);
    }
}
