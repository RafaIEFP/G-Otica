using FluentValidation;
using GOtica.Communication.Requests.Prescription;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.Prescription.Register;

internal class RegisterPrescriptionValidator : AbstractValidator<RequestRegisterPrescription>
{
    public RegisterPrescriptionValidator()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        RuleFor(request => request.DoctorName).NotEmpty().WithMessage(ResourceMessagesException.DOCTOR_NAME_EMPTY);

        RuleFor(request => request.DoctorRegistration).NotEmpty().WithMessage(ResourceMessagesException.DOCTOR_REGISTRATION_EMPTY);

        RuleFor(request => request.PrescriptionDate).LessThanOrEqualTo(today).WithMessage(ResourceMessagesException.PRESCRIPTION_DATE_INVALID);

        RuleFor(request => request.ExpirationDate).GreaterThanOrEqualTo(today).WithMessage(ResourceMessagesException.EXPIRATION_DATE_INVALID);

        RuleFor(request => request.RightEyeVisualAcuity)
            .MaximumLength(20)
            .WithMessage(ResourceMessagesException.VISUAL_ACUITY_MAX_LENGTH);

        RuleFor(request => request.LeftEyeVisualAcuity)
            .MaximumLength(20)
            .WithMessage(ResourceMessagesException.VISUAL_ACUITY_MAX_LENGTH);

        RuleFor(request => request.NearVisualAcuity)
            .MaximumLength(20)
            .WithMessage(ResourceMessagesException.VISUAL_ACUITY_MAX_LENGTH);

        RuleFor(request => request.Notes)
            .MaximumLength(1000)
            .WithMessage(ResourceMessagesException.NOTES_MAX_LENGTH);
    }
}
