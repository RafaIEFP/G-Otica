using GOtica.Communication.Requests.Prescription;
using GOtica.Communication.Response.Prescription;

namespace GOtica.Application.UseCases.Prescription.Register;

public interface IRegisterPrescriptionUseCase
{
    Task<ResponseRegisterPrescription> Execute(Guid opticalStoreId, Guid clientId, RequestRegisterPrescription request);
}
