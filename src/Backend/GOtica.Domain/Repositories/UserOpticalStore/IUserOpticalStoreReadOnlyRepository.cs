namespace GOtica.Domain.Repositories.UserOpticalStore;

public interface IUserOpticalStoreReadOnlyRepository
{
    Task<IReadOnlyCollection<Entities.UserOpticalStore>> GetUserOpticalStores(Guid userId);
    Task<bool> UserIsOwner(Guid userId);
    Task<bool> UserIsOwnerOfOpticalStore(Guid userId, long opticalId);
    Task<bool> UserBelongsToOptical(Guid userId, long opticalId);
    Task<string> GetUserOpticalRole(Guid userId, long opticalId);
}
