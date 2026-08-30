using GOtica.Communication.Response.Prescription;
using GOtica.Domain.Repositories.Prescription;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;
using Mapster;

namespace GOtica.Application.UseCases.Prescription.Get;

public class GetPrescriptionUseCase : IGetPrescriptionUseCase
{
    private readonly IPrescriptionReadOnlyRepository _prescriptionReadOnlyRepository;
    public GetPrescriptionUseCase(IPrescriptionReadOnlyRepository prescriptionReadOnlyRepository)
    {
        _prescriptionReadOnlyRepository = prescriptionReadOnlyRepository;
    }

    public async Task<ResponseGetPrescription> Execute(Guid opticalStoreId, Guid clientId, Guid prescriptionId)
    {
        var prescription = await _prescriptionReadOnlyRepository.GetById(prescriptionId, clientId, opticalStoreId)
            ?? 
            throw new NotFoundException(ResourceMessagesException.PRESCRIPTION_NOT_FOUND);

        return prescription.Adapt<ResponseGetPrescription>();
    }
}
