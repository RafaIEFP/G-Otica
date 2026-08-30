using GOtica.Communication.Requests;
using GOtica.Communication.Requests.Prescription;
using GOtica.Communication.Response.Prescription;
using GOtica.Domain.Repositories;
using GOtica.Domain.Repositories.Client;
using GOtica.Domain.Repositories.Prescription;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;
using Mapster;

namespace GOtica.Application.UseCases.Prescription.Register;

public class RegisterPrescriptionUseCase : IRegisterPrescriptionUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClientReadOnlyRepository _clientReadOnlyRepository;
    private readonly IPrescriptionWriteOnlyRepository _prescriptionWriteOnlyRepository;
    public RegisterPrescriptionUseCase(
        IUnitOfWork unitOfWork,
        IClientReadOnlyRepository clientReadOnlyRepository,
        IPrescriptionWriteOnlyRepository prescriptionWriteOnlyRepository)
    {
        _unitOfWork = unitOfWork;
        _clientReadOnlyRepository = clientReadOnlyRepository;
        _prescriptionWriteOnlyRepository = prescriptionWriteOnlyRepository;
    }

    public async Task<ResponseRegisterPrescription> Execute(Guid opticalStoreId, Guid clientId, RequestRegisterPrescription request)
    {
        request = request.Normalize();

        Validate(request);

        var clientExist = await _clientReadOnlyRepository.ExistActive(clientId, opticalStoreId);
        
        if (!clientExist)
            throw new NotFoundException(ResourceMessagesException.CLIENT_NOT_FOUND);

        var prescription = request.Adapt<Domain.Entities.Prescription>();
        prescription.ClientId = clientId;

        await _prescriptionWriteOnlyRepository.Add(prescription);

        await _unitOfWork.Commit();

        return new ResponseRegisterPrescription
        {
            Id = prescription.Id
        };
    }

    private static void Validate(RequestRegisterPrescription request)
    {
        var result = new RegisterPrescriptionValidator().Validate(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }
}
