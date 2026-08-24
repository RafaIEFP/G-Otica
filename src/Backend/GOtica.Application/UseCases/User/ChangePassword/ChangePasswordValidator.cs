using FluentValidation;
using GOtica.Application.SharedValidators;
using GOtica.Communication.Requests.User;

namespace GOtica.Application.UseCases.User.ChangePassword;

public class ChangePasswordValidator : AbstractValidator<RequestChangePassword>
{
    public ChangePasswordValidator()
    {
        RuleFor(r => r.NewPassword).Password();
    }
}
