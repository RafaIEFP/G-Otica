using FluentValidation;
using GOtica.Communication.Requests.UserOpticalStore;
using GOtica.Domain;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.UserOpticalStores.ChangeRole;

public class ChangeRoleValidator : AbstractValidator<RequestChangeRole>
{
    public ChangeRoleValidator()
    {
        RuleFor(request => request.Role)
            .Must(role =>
                role == Roles.MANAGER ||
                role == Roles.SALESPERSON)
            .WithMessage(ResourceMessagesException.ROLE_INVALID);
    }
}
