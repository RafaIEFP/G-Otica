namespace GOtica.Domain.Repositories.UserOpticalStore;

public interface IUserOpticalStoreUpdateOnlyRepository
{
    Task UpdateUserRoleOpticalStoreAssociation(Guid userId, Guid opticalId, string newRole);
    Task DeactivateByUser(Guid userId);
    Task DeactivateByOpticalStore(Guid opticalStoreId);
}
