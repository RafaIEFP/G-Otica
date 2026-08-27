using GOtica.Application.UseCases.OpticalStores.Register;
using GOtica.Communication.Requests;
using GOtica.Communication.Requests.OpticalStore;
using GOtica.Domain.Repositories;
using GOtica.Domain.Repositories.OpticalStore;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;
using Mapster;

namespace GOtica.Application.UseCases.OpticalStores.Update;

public class UpdateOpticalStoreUseCase : IUpdateOpticalStoreUseCase
{
    private readonly IOpticalStoreReadOnlyRepository _opticalStoreReadOnlyRepository;
    private readonly IOpticalStoreUpdateOnlyRepository _opticalStoreUpdateOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateOpticalStoreUseCase(
        IOpticalStoreReadOnlyRepository opticalStoreReadOnlyRepository,
        IOpticalStoreUpdateOnlyRepository opticalStoreUpdateOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _opticalStoreReadOnlyRepository = opticalStoreReadOnlyRepository;
        _opticalStoreUpdateOnlyRepository = opticalStoreUpdateOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid opticalStoreId, RequestOpticalStore request)
    {
        request = request.Normalize();

        Validate(request);

        var opticalStore = await _opticalStoreUpdateOnlyRepository.GetById(opticalStoreId);

        if (request.TaxNumber != opticalStore.TaxNumber)
        {
            var opticalStoreAlreadyExists = await _opticalStoreReadOnlyRepository.ExistOpticalStoreRegistered(request.TaxNumber);

            if (opticalStoreAlreadyExists)
                throw new ConflictException(ResourceMessagesException.OPTICAL_STORE_ALREADY_REGISTERED);
        }

        opticalStore = request.Adapt(opticalStore);

        _opticalStoreUpdateOnlyRepository.Update(opticalStore);

        await _unitOfWork.Commit();
    }

    private void Validate(RequestOpticalStore request)
    {
        var result = new RegisterOpticalStoreValidator().Validate(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }
}
