using GOtica.Communication.Response;
using GOtica.Domain.Repositories.UserOpticalStore;
using GOtica.Domain.Services;
using Mapster;

namespace GOtica.Application.UseCases.User.Profile;

public class GetUserProfileUseCase : IGetUserProfileUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserOpticalStoreReadOnlyRepository _userOpticalStoreReadOnlyRepository;
    public GetUserProfileUseCase(
        ILoggedUser loggedUser,
        IUserOpticalStoreReadOnlyRepository userOpticalStoreReadOnlyRepository)
    {
        _loggedUser = loggedUser;
        _userOpticalStoreReadOnlyRepository = userOpticalStoreReadOnlyRepository;
    }
    public async Task<ResponseUserProfile> Execute()
    {
        var loggedUser = await _loggedUser.Get();

        var userOpticalStores = await _userOpticalStoreReadOnlyRepository.GetUserOpticalStores(loggedUser.Id);

        return loggedUser.Adapt<ResponseUserProfile>();
    }
}
