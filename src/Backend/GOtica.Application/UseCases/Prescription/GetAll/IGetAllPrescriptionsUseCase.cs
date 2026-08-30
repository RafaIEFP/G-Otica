using GOtica.Communication.Requests.Prescription;
using GOtica.Communication.Response;
using GOtica.Communication.Response.Prescription;

namespace GOtica.Application.UseCases.Prescription.GetAll;

public interface IGetAllPrescriptionsUseCase
{
    Task<ResponsePaged<ResponseGetAllPrescription>> Execute(Guid opticalStoreId, Guid clientId, RequestGetAllPrescriptions request);
}
