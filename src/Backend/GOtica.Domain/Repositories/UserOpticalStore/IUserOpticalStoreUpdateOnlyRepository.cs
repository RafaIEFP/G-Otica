namespace GOtica.Domain.Repositories.UserOpticalStore;

public interface IUserOpticalStoreUpdateOnlyRepository
{
    Task UpdateUserRoleOpticalStore(Guid userId, Guid opticalId, string newRole);
    Task DeactivateUserOpticalStores(Guid userId);
}
