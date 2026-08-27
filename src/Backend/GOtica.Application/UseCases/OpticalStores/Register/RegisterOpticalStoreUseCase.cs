using GOtica.Communication.Requests;
using GOtica.Communication.Requests.OpticalStore;
using GOtica.Communication.Response.OpticalStore;
using GOtica.Domain;
using GOtica.Domain.Entities;
using GOtica.Domain.Repositories;
using GOtica.Domain.Repositories.OpticalStore;
using GOtica.Domain.Repositories.UserOpticalStore;
using GOtica.Domain.Services;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;
using Mapster;

namespace GOtica.Application.UseCases.OpticalStores.Register;

public class RegisterOpticalStoreUseCase : IRegisterOpticalStoreUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOpticalStoreReadOnlyRepository _opticalStoreReadOnlyRepository;
    private readonly IOpticalStoreWriteOnlyRepository _opticalStoreWriteOnlyRepository;
    private readonly IUserOpticalStoreWriteOnlyRepository _userOpticalStoreWriteOnlyRepository;

    public RegisterOpticalStoreUseCase(
        ILoggedUser loggedUser,
        IUnitOfWork unitOfWork,
        IOpticalStoreReadOnlyRepository opticalStoreReadOnlyRepository,
        IOpticalStoreWriteOnlyRepository opticalStoreWriteOnlyRepository,
        IUserOpticalStoreWriteOnlyRepository userOpticalStoreWriteOnlyRepository)
    {
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
        _opticalStoreReadOnlyRepository = opticalStoreReadOnlyRepository;
        _opticalStoreWriteOnlyRepository = opticalStoreWriteOnlyRepository;
        _userOpticalStoreWriteOnlyRepository = userOpticalStoreWriteOnlyRepository;
    }

    public async Task<ResponseRegisterOpticalStore> Execute(RequestOpticalStore request)
    {
        request = request.Normalize();

        var loggedUser = await _loggedUser.Get();

        Vaidate(request);

        var opticalAlreadyExists = await _opticalStoreReadOnlyRepository.ExistOpticalStoreRegistered(request.TaxNumber);

        if (opticalAlreadyExists)
            throw new ConflictException(ResourceMessagesException.OPTICAL_STORE_ALREADY_REGISTERED);

        var opticalStore = request.Adapt<OpticalStore>();

        await _opticalStoreWriteOnlyRepository.Add(opticalStore);

        await _userOpticalStoreWriteOnlyRepository.Add(new Domain.Entities.UserOpticalStore
        {
            EntranceDate = DateOnly.FromDateTime(DateTime.UtcNow),
            Role = Roles.OWNER,
            UserId = loggedUser.Id,
            OpticalStoreId = opticalStore.Id
        });

        await _unitOfWork.Commit();

        return opticalStore.Adapt<ResponseRegisterOpticalStore>();
    }

    private static void Vaidate(RequestOpticalStore request)
    {
        var result = new RegisterOpticalStoreValidator().Validate(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }
}
