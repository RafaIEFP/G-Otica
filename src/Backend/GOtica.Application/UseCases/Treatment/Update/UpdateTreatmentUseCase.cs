using GOtica.Communication.Requests;
using GOtica.Communication.Requests.Treatment;
using GOtica.Domain.Repositories;
using GOtica.Domain.Repositories.Treatment;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;
using Mapster;

namespace GOtica.Application.UseCases.Treatment.Update;

public class UpdateTreatmentUseCase : IUpdateTreatmentUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITreatmentReadOnlyRepository _treatmentReadOnlyRepository;
    private readonly ITreatmentUpdateOnlyRepository _treatmentUpdateOnlyRepository;
    public UpdateTreatmentUseCase(
        IUnitOfWork unitOfWork,
        ITreatmentReadOnlyRepository treatmentReadOnlyRepository,
        ITreatmentUpdateOnlyRepository treatmentUpdateOnlyRepository)
    {
        _unitOfWork = unitOfWork;
        _treatmentReadOnlyRepository = treatmentReadOnlyRepository;
        _treatmentUpdateOnlyRepository = treatmentUpdateOnlyRepository;
    }

    public async Task Execute(Guid opticalStoreId, Guid treatmentId, RequestTreatment request)
    {
        request = request.Normalize();

        Validate(request);

        var treatment = await _treatmentUpdateOnlyRepository.GetActiveInOpticalStore(treatmentId, opticalStoreId)
            ??
            throw new NotFoundException(ResourceMessagesException.TREATMENT_NOT_FOUND);

        var treatmentAlreadyRegistered =
            await _treatmentReadOnlyRepository.TreatmentAlreadyAtOpticalStore(request.Name, opticalStoreId, treatmentId);

        if (treatmentAlreadyRegistered)
            throw new ConflictException(ResourceMessagesException.TREATMENT_ALREADY_REGISTERED);

        request.Adapt(treatment);

        await _unitOfWork.Commit();
    }

    private static void Validate(RequestTreatment request)
    {
        var result = new TreatmentValidator().Validate(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException(result.Errors.Select(error => error.ErrorMessage).ToList());
    }
}
