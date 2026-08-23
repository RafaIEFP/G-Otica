using GOtica.Communication.Response.UserOpticalStore;
using GOtica.Domain.Repositories.UserOpticalStore;
using Mapster;

namespace GOtica.Application.UseCases.UserOpticalStores.GetAll;

public class GetAllOpticalStoreUsersUseCase : IGetAllOpticalStoreUsersUseCase
{
    private readonly IUserOpticalStoreReadOnlyRepository _userOpticalStoreReadOnlyRepository;
    public GetAllOpticalStoreUsersUseCase(
        IUserOpticalStoreReadOnlyRepository userOpticalStoreReadOnlyRepository)
    {
        _userOpticalStoreReadOnlyRepository = userOpticalStoreReadOnlyRepository;
    }

    public async Task<IReadOnlyCollection<ResponseGetAllOpticalStoreUser>> Execute(Guid opticalStoreId)
    {
        var users = await _userOpticalStoreReadOnlyRepository.GetAllOpticalStoreUsers(opticalStoreId);

        return users.Adapt<IReadOnlyCollection<ResponseGetAllOpticalStoreUser>>();
    }
}
