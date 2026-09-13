using GOtica.Communication.Requests.Treatment;
using GOtica.Communication.Response;
using GOtica.Communication.Response.Treatment;

namespace GOtica.Application.UseCases.Treatment.GetAll;

public interface IGetAllTreatmentsUseCase
{
    Task<ResponsePaged<ResponseTreatment>> Execute(Guid opticalStoreId, RequestGetAllTreatments request);
}
