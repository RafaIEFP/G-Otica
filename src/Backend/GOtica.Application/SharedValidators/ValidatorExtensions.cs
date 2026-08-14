using FluentValidation;

namespace GOtica.Application.SharedValidators;

public static class ValidatorExtensions
{
    extension<T>(IRuleBuilder<T, string> ruleBuilder)
    {
        public IRuleBuilderOptions<T, string> Password() => ruleBuilder.SetValidator(new PasswordValidator<T>());

        public IRuleBuilderOptions<T, string> PhoneNumber() => ruleBuilder.SetValidator(new PhoneNumberValidator<T>());
    }
}
