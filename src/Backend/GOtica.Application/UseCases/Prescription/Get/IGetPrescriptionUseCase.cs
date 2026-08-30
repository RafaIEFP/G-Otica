using GOtica.Communication.Response.Prescription;

namespace GOtica.Application.UseCases.Prescription.Get;

public interface IGetPrescriptionUseCase
{
    Task<ResponseGetPrescription> Execute(Guid opticalStoreId, Guid clientId, Guid prescriptionId);
}
