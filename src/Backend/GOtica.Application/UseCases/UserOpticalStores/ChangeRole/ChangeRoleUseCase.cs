using GOtica.Communication.Requests;
using GOtica.Communication.Requests.UserOpticalStore;
using GOtica.Domain;
using GOtica.Domain.Repositories.UserOpticalStore;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.UserOpticalStores.ChangeRole;

public class ChangeRoleUseCase : IChangeRoleUseCase
{
    private readonly IUserOpticalStoreReadOnlyRepository _userOpticalStoreReadOnlyRepository;
    private readonly IUserOpticalStoreUpdateOnlyRepository _userOpticalStoreUpdateOnlyRepository;
    public ChangeRoleUseCase(
        IUserOpticalStoreReadOnlyRepository userOpticalStoreReadOnlyRepository,
        IUserOpticalStoreUpdateOnlyRepository userOpticalStoreUpdateOnlyRepository)
    {
        _userOpticalStoreReadOnlyRepository = userOpticalStoreReadOnlyRepository;
        _userOpticalStoreUpdateOnlyRepository = userOpticalStoreUpdateOnlyRepository;
    }

    public async Task Execute(Guid opticalStoreId, Guid userId, RequestChangeRole request)
    {
        request = request.Normalize();

        Validate(request);

        var userOpticalStore = await _userOpticalStoreReadOnlyRepository.GetUserOpticalStore(userId, opticalStoreId)
            ?? throw new NotFoundException(ResourceMessagesException.USER_NOT_FOUND);

        if (userOpticalStore.Role == Roles.OWNER)
            throw new ConflictException(ResourceMessagesException.OWNER_ROLE_CANNOT_BE_CHANGED);

        if (userOpticalStore.Role == request.Role) 
            return;

        await _userOpticalStoreUpdateOnlyRepository.UpdateUserRoleOpticalStoreAssociation(userId, opticalStoreId, request.Role);
    }

    private void Validate(RequestChangeRole request)
    {
        var result = new ChangeRoleValidator().Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
