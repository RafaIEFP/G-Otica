using GOtica.Communication.Requests.Treatment;
using GOtica.Communication.Response.Treatment;

namespace GOtica.Application.UseCases.Treatment.Register;

public interface IRegisterTreatmentUseCase
{
    Task<ResponseRegisterTreatment> Execute(Guid opticalStoreId, RequestRegisterTreatment request);
}
