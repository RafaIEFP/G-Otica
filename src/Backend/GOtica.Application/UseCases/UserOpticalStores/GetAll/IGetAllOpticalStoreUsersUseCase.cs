using GOtica.Communication.Response.UserOpticalStore;

namespace GOtica.Application.UseCases.UserOpticalStores.GetAll;

public interface IGetAllOpticalStoreUsersUseCase
{
    Task<IReadOnlyCollection<ResponseGetAllOpticalStoreUser>> Execute(Guid opticalStoreId);
}
