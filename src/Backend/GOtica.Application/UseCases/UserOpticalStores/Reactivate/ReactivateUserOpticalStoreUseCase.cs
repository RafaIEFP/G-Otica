using GOtica.Domain.Repositories.UserOpticalStore;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.UserOpticalStores.Reactivate;

public class ReactivateUserOpticalStoreUseCase : IReactivateUserOpticalStoreUseCase
{
    private readonly IUserOpticalStoreUpdateOnlyRepository _userOpticalStoreUpdateOnlyRepository;
    private readonly IUserOpticalStoreReadOnlyRepository _userOpticalStoreReadOnlyRepository;
    public ReactivateUserOpticalStoreUseCase(
        IUserOpticalStoreUpdateOnlyRepository userOpticalStoreUpdateOnlyRepository,
        IUserOpticalStoreReadOnlyRepository userOpticalStoreReadOnlyRepository)
    {
        _userOpticalStoreUpdateOnlyRepository = userOpticalStoreUpdateOnlyRepository;
        _userOpticalStoreReadOnlyRepository = userOpticalStoreReadOnlyRepository;
    }

    public async Task Execute(Guid opticalStoreId, Guid userId)
    {
        var userOpticalStore = await _userOpticalStoreReadOnlyRepository.GetInactiveUserOpticalStore(userId, opticalStoreId)
            ?? 
            throw new NotFoundException(ResourceMessagesException.OPTICAL_STORE_MEMBER_NOT_FOUND);

        if (!userOpticalStore.User.IsActive)
            throw new ConflictException(ResourceMessagesException.USER_ACCOUNT_IS_INACTIVE);

        await _userOpticalStoreUpdateOnlyRepository.Reactivate(userId, opticalStoreId);
    }
}
