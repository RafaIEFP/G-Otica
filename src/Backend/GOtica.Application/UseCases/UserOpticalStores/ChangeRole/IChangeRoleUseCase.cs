using GOtica.Communication.Requests;

namespace GOtica.Application.UseCases.UserOpticalStores.ChangeRole;

public interface IChangeRoleUseCase
{
    Task Execute(Guid opticalStoreId, Guid userId, RequestChangeRole request);
}
