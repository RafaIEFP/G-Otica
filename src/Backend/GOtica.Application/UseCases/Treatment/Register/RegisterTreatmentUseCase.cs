using GOtica.Communication.Requests;
using GOtica.Communication.Requests.Treatment;
using GOtica.Communication.Response.Treatment;
using GOtica.Domain.Repositories;
using GOtica.Domain.Repositories.Treatment;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;
using Mapster;

namespace GOtica.Application.UseCases.Treatment.Register;

public class RegisterTreatmentUseCase : IRegisterTreatmentUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITreatmentReadOnlyRepository _treatmentReadOnlyRepository;
    private readonly ITreatmentWriteOnlyRepository _treatmentWriteOnlyRepository;
    public RegisterTreatmentUseCase(
        IUnitOfWork unitOfWork,
        ITreatmentReadOnlyRepository treatmentReadOnlyRepository,
        ITreatmentWriteOnlyRepository treatmentWriteOnlyRepository)
    {
        _unitOfWork = unitOfWork;
        _treatmentReadOnlyRepository = treatmentReadOnlyRepository;
        _treatmentWriteOnlyRepository = treatmentWriteOnlyRepository;
    }

    public async Task<ResponseRegisterTreatment> Execute(Guid opticalStoreId, RequestRegisterTreatment request)
    {
        request = request.Normalize();

        Validate(request);

        var treatmentAlreadyRegistered = await _treatmentReadOnlyRepository.TreatmentAlreadyAtOpticalStore(request.Name, opticalStoreId);

        if (treatmentAlreadyRegistered)
            throw new ConflictException(ResourceMessagesException.TREATMENT_ALREADY_REGISTERED);

        var treatment = request.Adapt<Domain.Entities.Treatment>();

        treatment.OpticalStoreId = opticalStoreId;

        await _treatmentWriteOnlyRepository.Add(treatment);

        await _unitOfWork.Commit();

        return treatment.Adapt<ResponseRegisterTreatment>();
    }

    private static void Validate(RequestRegisterTreatment request)
    {
        var result = new RegisterTreatmentValidator().Validate(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException(result.Errors.Select(error => error.ErrorMessage).ToList());
    }
}
