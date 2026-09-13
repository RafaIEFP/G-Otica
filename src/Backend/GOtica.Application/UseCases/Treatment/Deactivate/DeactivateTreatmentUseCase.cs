using GOtica.Domain.Repositories.Treatment;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.Treatment.Deactivate;

public class DeactivateTreatmentUseCase : IDeactivateTreatmentUseCase
{
    private readonly ITreatmentUpdateOnlyRepository _treatmentUpdateOnlyRepository;

    public DeactivateTreatmentUseCase(ITreatmentUpdateOnlyRepository treatmentUpdateOnlyRepository)
    {
        _treatmentUpdateOnlyRepository = treatmentUpdateOnlyRepository;
    }

    public async Task Execute(Guid opticalStoreId, Guid treatmentId)
    {
        var deactivated = await _treatmentUpdateOnlyRepository.Deactivate(treatmentId, opticalStoreId);

        if (!deactivated)
            throw new NotFoundException(ResourceMessagesException.TREATMENT_NOT_FOUND);
    }
}
