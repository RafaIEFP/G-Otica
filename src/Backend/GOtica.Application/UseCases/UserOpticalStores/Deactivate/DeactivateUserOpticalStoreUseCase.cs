using GOtica.Domain;
using GOtica.Domain.Repositories.UserOpticalStore;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.UserOpticalStores.Deactivate;

public class DeactivateUserOpticalStoreUseCase : IDeactivateUserOpticalStoreUseCase
{
    private readonly IUserOpticalStoreReadOnlyRepository _userOpticalStoreReadOnlyRepository;
    private readonly IUserOpticalStoreUpdateOnlyRepository _userOpticalStoreUpdateOnlyRepository;
    public DeactivateUserOpticalStoreUseCase(
        IUserOpticalStoreReadOnlyRepository userOpticalStoreReadOnlyRepository,
        IUserOpticalStoreUpdateOnlyRepository userOpticalStoreUpdateOnlyRepository)
    {
        _userOpticalStoreReadOnlyRepository = userOpticalStoreReadOnlyRepository;
        _userOpticalStoreUpdateOnlyRepository = userOpticalStoreUpdateOnlyRepository;
    }

    public async Task Execute(Guid opticalStoreId, Guid userId)
    {
        var userOpticalStore = await _userOpticalStoreReadOnlyRepository.GetUserOpticalStore(userId, opticalStoreId)
            ?? throw new NotFoundException(ResourceMessagesException.OPTICAL_STORE_MEMBER_NOT_FOUND);

        if (userOpticalStore.Role == Roles.OWNER)
            throw new ConflictException(ResourceMessagesException.OWNER_CANNOT_BE_DEACTIVATED);

        await _userOpticalStoreUpdateOnlyRepository.DeactivateByUserAndOpticalStore(userId, opticalStoreId);
    }
}
