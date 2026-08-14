using FluentValidation;
using GOtica.Application.SharedValidators;
using GOtica.Communication.Requests;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.OpticalStores.Register;

public class RegisterOpticalStoreValidator : AbstractValidator<RequestRegisterOpticalStore>
{
    public RegisterOpticalStoreValidator()
    {
        RuleFor(r => r.Name).NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY);
        RuleFor(r => r.Email)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.EMAIL_EMPTY)
            .EmailAddress()
            .When(r => !string.IsNullOrWhiteSpace(r.Email), ApplyConditionTo.CurrentValidator)
            .WithMessage(ResourceMessagesException.EMAIL_INVALID);
        RuleFor(r => r.PhoneNumber).PhoneNumber();
        RuleFor(r => r.TaxNumber).NotEmpty().WithMessage(ResourceMessagesException.TAX_NUMBER_EMPTY);
    }
}
