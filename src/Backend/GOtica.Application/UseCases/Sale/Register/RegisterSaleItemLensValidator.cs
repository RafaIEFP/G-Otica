using FluentValidation;
using GOtica.Communication.Requests.Sale;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.Sale.Register;

internal class RegisterSaleItemLensValidator : AbstractValidator<RequestRegisterSaleItemLens>
{
    public RegisterSaleItemLensValidator()
    {
        RuleFor(lens => lens.EyeSide).NotNull().WithMessage(ResourceMessagesException.EYE_SIDE_REQUIRED);

        RuleFor(lens => lens.EyeSide)
            .IsInEnum()
            .When(lens => lens.EyeSide.HasValue)
            .WithMessage(ResourceMessagesException.EYE_SIDE_INVALID);

        RuleFor(lens => lens.LensType).NotNull().WithMessage(ResourceMessagesException.LENS_TYPE_REQUIRED);

        RuleFor(lens => lens.LensType)
            .IsInEnum()
            .When(lens => lens.LensType.HasValue)
            .WithMessage(ResourceMessagesException.LENS_TYPE_INVALID);

        RuleFor(lens => lens.Material).NotNull().WithMessage(ResourceMessagesException.LENS_MATERIAL_REQUIRED);

        RuleFor(lens => lens.Material)
            .IsInEnum()
            .When(lens => lens.Material.HasValue)
            .WithMessage(ResourceMessagesException.LENS_MATERIAL_INVALID);

        RuleFor(lens => lens.RefractiveIndex).GreaterThan(0).WithMessage(ResourceMessagesException.REFRACTIVE_INDEX_INVALID);

        RuleFor(lens => lens.Diameter).GreaterThan(0).WithMessage(ResourceMessagesException.LENS_DIAMETER_INVALID);

        RuleFor(lens => lens.PupillaryDistance)
            .GreaterThan(0)
            .When(lens => lens.PupillaryDistance.HasValue)
            .WithMessage(ResourceMessagesException.PUPILLARY_DISTANCE_INVALID);

        RuleFor(lens => lens.NasoPupillaryDistance)
            .GreaterThan(0)
            .When(lens => lens.NasoPupillaryDistance.HasValue)
            .WithMessage(ResourceMessagesException.NASO_PUPILLARY_DISTANCE_INVALID);

        RuleFor(lens => lens.Color).MaximumLength(100).WithMessage(ResourceMessagesException.LENS_COLOR_MAX_LENGTH);

        RuleForEach(lens => lens.TreatmentIds).NotEmpty().WithMessage(ResourceMessagesException.LENS_TREATMENTS_INVALID);

        RuleFor(lens => lens.TreatmentIds)
            .Must(treatmentIds =>
                treatmentIds is null ||
                treatmentIds.Count == treatmentIds.Distinct().Count())
            .WithMessage(ResourceMessagesException.LENS_TREATMENTS_DUPLICATED);
    }
}
