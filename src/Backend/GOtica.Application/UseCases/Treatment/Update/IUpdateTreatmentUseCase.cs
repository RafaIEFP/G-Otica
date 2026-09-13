using GOtica.Communication.Requests.Treatment;

namespace GOtica.Application.UseCases.Treatment.Update;

public interface IUpdateTreatmentUseCase
{
    Task Execute(Guid opticalStoreId, Guid treatmentId, RequestTreatment request);
}
