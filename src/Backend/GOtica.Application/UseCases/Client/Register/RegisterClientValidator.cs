using FluentValidation;
using GOtica.Application.SharedValidators;
using GOtica.Communication.Requests.Client;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.Client.Register;

public class RegisterClientValidator : AbstractValidator<RequestRegisterClient>
{
    public RegisterClientValidator()
    {
        RuleFor(request => request.Name).NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY);

        RuleFor(request => request.Email).EmailAddress()
            .When(client => !string.IsNullOrWhiteSpace(client.Email))
            .WithMessage(ResourceMessagesException.EMAIL_INVALID);

        RuleFor(request => request.PhoneNumber).PhoneNumber().WithMessage(ResourceMessagesException.PHONE_NUMBER_INVALID);

        RuleFor(client => client.DateOfBirth)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .When(client => client.DateOfBirth.HasValue)
            .WithMessage(ResourceMessagesException.DATE_OF_BIRTH_IN_THE_FUTURE);
    }
}
