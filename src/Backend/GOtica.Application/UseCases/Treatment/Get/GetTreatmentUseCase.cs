using GOtica.Communication.Response.Treatment;
using GOtica.Domain.Repositories.Treatment;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;
using Mapster;

namespace GOtica.Application.UseCases.Treatment.Get;

public class GetTreatmentUseCase : IGetTreatmentUseCase
{
    private readonly ITreatmentReadOnlyRepository _treatmentReadOnlyRepository;
    public GetTreatmentUseCase(
        ITreatmentReadOnlyRepository treatmentReadOnlyRepository)
    {
        _treatmentReadOnlyRepository = treatmentReadOnlyRepository;
    }

    public async Task<ResponseTreatment> Execute(Guid opticalStoreId, Guid treatmentId)
    {
        var treatment = await _treatmentReadOnlyRepository.GetById(treatmentId, opticalStoreId)
            ??
            throw new NotFoundException(ResourceMessagesException.TREATMENT_NOT_FOUND);

        return treatment.Adapt<ResponseTreatment>();
    }
}
