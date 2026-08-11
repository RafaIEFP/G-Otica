namespace GOtica.Domain.Repositories.UserOpticalStore;

public interface IUserOpticalStoreUpdateOnlyRepository
{
    Task UpdateUserRoleOpticalStore(Guid userId, long opticalId, string newRole);
}
