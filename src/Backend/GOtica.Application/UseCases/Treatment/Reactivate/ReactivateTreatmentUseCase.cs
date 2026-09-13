using GOtica.Domain.Repositories.Treatment;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.Treatment.Reactivate;

public class ReactivateTreatmentUseCase : IReactivateTreatmentUseCase
{
    private readonly ITreatmentUpdateOnlyRepository _treatmentUpdateOnlyRepository;

    public ReactivateTreatmentUseCase(ITreatmentUpdateOnlyRepository treatmentUpdateOnlyRepository)
    {
        _treatmentUpdateOnlyRepository = treatmentUpdateOnlyRepository;
    }

    public async Task Execute(Guid opticalStoreId, Guid treatmentId)
    {
        var reactivated = await _treatmentUpdateOnlyRepository.Reactivate(treatmentId, opticalStoreId);

        if (!reactivated)
            throw new NotFoundException(ResourceMessagesException.TREATMENT_NOT_FOUND);
    }
}
