using GOtica.Communication.Response.Treatment;

namespace GOtica.Application.UseCases.Treatment.Get;

public interface IGetTreatmentUseCase
{
    Task<ResponseTreatment> Execute(Guid opticalStoreId, Guid treatmentId);
}
