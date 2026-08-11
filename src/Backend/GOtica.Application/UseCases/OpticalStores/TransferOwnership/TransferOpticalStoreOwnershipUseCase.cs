using GOtica.Domain;
using GOtica.Domain.Repositories;
using GOtica.Domain.Repositories.OpticalStore;
using GOtica.Domain.Repositories.User;
using GOtica.Domain.Repositories.UserOpticalStore;
using GOtica.Domain.Services;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.OpticalStores.TransferOwnership;

public class TransferOpticalStoreOwnershipUseCase : ITransferOpticalStoreOwnershipUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IOpticalStoreReadOnlyRepository _opticalStoreReadOnlyRepository;
    private readonly IUserOpticalStoreReadOnlyRepository _userOpticalStoreReadOnlyRepository;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUserOpticalStoreUpdateOnlyRepository _userOpticalStoreUpdateOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    public TransferOpticalStoreOwnershipUseCase(
        ILoggedUser loggedUser,
        IOpticalStoreReadOnlyRepository opticalStoreReadOnlyRepository,
        IUserOpticalStoreReadOnlyRepository userOpticalStoreReadOnlyRepository,
        IUserReadOnlyRepository userReadOnlyRepository,
        IUserOpticalStoreUpdateOnlyRepository userOpticalStoreUpdateOnlyRepository,
        IUnitOfWork unitOfWork
        )
    {
        _loggedUser = loggedUser;
        _opticalStoreReadOnlyRepository = opticalStoreReadOnlyRepository;
        _userOpticalStoreReadOnlyRepository = userOpticalStoreReadOnlyRepository;
        _userReadOnlyRepository = userReadOnlyRepository;
        _userOpticalStoreUpdateOnlyRepository = userOpticalStoreUpdateOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid newOwnerUserId, long opticalId)
    {
        var loggedUser = await _loggedUser.Get();

        await Validate(newOwnerUserId, opticalId, loggedUser.Id);

        await _unitOfWork.ExecuteInTransaction(async() =>
        {
            await _userOpticalStoreUpdateOnlyRepository.UpdateUserRoleOpticalStore(loggedUser.Id, opticalId, Roles.MANAGER);
            await _userOpticalStoreUpdateOnlyRepository.UpdateUserRoleOpticalStore(newOwnerUserId, opticalId, Roles.OWNER);
        });
    }

    private async Task Validate(Guid newOwnerUserId, long opticalId, Guid loggedUserId)
    {
        if (loggedUserId == newOwnerUserId)
            throw new ConflictException(ResourceMessagesException.USER_ALREADY_OWNER);

        // 1. Ótica existe?
        var opticalStoreExists = await _opticalStoreReadOnlyRepository.ExistsActiveOptical(opticalId);

        if (!opticalStoreExists)
            throw new NotFoundException(ResourceMessagesException.OPTICAL_STORE_NOT_FOUND);

        // 2. Solicitante pertence à ótica ?? (relaçao ativa) 
        var userBelongsToOpticalStore = await _userOpticalStoreReadOnlyRepository.UserBelongsToOptical(loggedUserId, opticalId);

        if (!userBelongsToOpticalStore)
            throw new UnauthorizedException(ResourceMessagesException.USER_DOES_NOT_BELONG_OPTICAL);

        // 3. Novo owner deve existir e estar ativo
        var newOwnerExists = await _userReadOnlyRepository.ExistsActiveUser(newOwnerUserId);

        if (!newOwnerExists)
            throw new NotFoundException(ResourceMessagesException.USER_NOT_FOUND);

        // 4. Novo owner deve estar associado à ótica.
        var newOwnerBelongsToOpticalStore = await _userOpticalStoreReadOnlyRepository.UserBelongsToOptical(newOwnerUserId, opticalId);

        if (!newOwnerBelongsToOpticalStore)
            throw new UnauthorizedException(ResourceMessagesException.NEW_OWNER_DOES_NOT_BELONG_OPTICAL);
    }
}
