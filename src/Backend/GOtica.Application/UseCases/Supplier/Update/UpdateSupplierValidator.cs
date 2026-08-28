using FluentValidation;
using GOtica.Application.SharedValidators;
using GOtica.Communication.Requests.Supplier;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.Supplier.Update;

public class UpdateSupplierValidator : AbstractValidator<RequestUpdateSupplier>
{
    public UpdateSupplierValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.NAME_EMPTY)
            .MaximumLength(255);

        RuleFor(request => request.Email)
            .EmailAddress()
            .When(request => !string.IsNullOrWhiteSpace(request.Email), ApplyConditionTo.CurrentValidator)
            .WithMessage(ResourceMessagesException.EMAIL_INVALID);

        RuleFor(request => request.PhoneNumber!)
            .PhoneNumber()
            .When(request => !string.IsNullOrWhiteSpace(request.PhoneNumber), ApplyConditionTo.CurrentValidator);
    }
}
