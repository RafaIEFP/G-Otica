using GOtica.Communication.Response.OpticalStore;
using GOtica.Domain.Repositories.UserOpticalStore;
using GOtica.Domain.Services;
using Mapster;

namespace GOtica.Application.UseCases.OpticalStores.GetAll;

public class GetAllOpticalStoresUseCase : IGetAllOpticalStoresUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserOpticalStoreReadOnlyRepository _userOpticalStoreReadOnlyRepository;
    public GetAllOpticalStoresUseCase(
        ILoggedUser loggedUser,
        IUserOpticalStoreReadOnlyRepository userOpticalStoreReadOnlyRepository)
    {
        _loggedUser = loggedUser;
        _userOpticalStoreReadOnlyRepository = userOpticalStoreReadOnlyRepository;
    }

    public async Task<IReadOnlyCollection<ResponseGetAllOpticalStores>> Execute()
    {
        var loggedUser = await _loggedUser.Get();

        var opticalStores = await _userOpticalStoreReadOnlyRepository.GetAllOpticalStoresWithRole(loggedUser.Id);

        return opticalStores.Adapt<IReadOnlyCollection<ResponseGetAllOpticalStores>>();
    }
}
