using GOtica.Communication.Response.OpticalStore;
using GOtica.Domain.Repositories.UserOpticalStore;
using GOtica.Domain.Services;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;
using Mapster;

namespace GOtica.Application.UseCases.OpticalStores.Get;

public class GetOpticalStoreUseCase : IGetOpticalStoreUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserOpticalStoreReadOnlyRepository _userOpticalStoreReadOnlyRepository;
    public GetOpticalStoreUseCase(
        ILoggedUser loggedUser,
        IUserOpticalStoreReadOnlyRepository userOpticalStoreReadOnlyRepository)
    {
        _loggedUser = loggedUser;
        _userOpticalStoreReadOnlyRepository = userOpticalStoreReadOnlyRepository;
    }

    public async Task<ResponseGetOpticalStore> Execute(Guid opticalStoreId)
    {
        var loggedUser = await _loggedUser.Get();

        var opticalStore = await _userOpticalStoreReadOnlyRepository.GetOpticalStoreWithRole(loggedUser.Id, opticalStoreId)
            ?? throw new NotFoundException(ResourceMessagesException.OPTICAL_STORE_NOT_FOUND);

        return opticalStore.Adapt<ResponseGetOpticalStore>();
    }
}
