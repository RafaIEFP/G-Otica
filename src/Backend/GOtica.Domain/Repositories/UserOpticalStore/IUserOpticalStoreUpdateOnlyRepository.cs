namespace GOtica.Domain.Repositories.UserOpticalStore;

public interface IUserOpticalStoreUpdateOnlyRepository
{
    Task UpdateUserRoleOpticalStoreAssociation(Guid userId, Guid opticalId, string newRole);
    Task DeactivateByUser(Guid userId);
    Task DeactivateByUserAndOpticalStore(Guid userId, Guid opticalStoreId);
    Task DeactivateByOpticalStore(Guid opticalStoreId);
    Task Reactivate(Guid userId, Guid opticalStoreId);
}
