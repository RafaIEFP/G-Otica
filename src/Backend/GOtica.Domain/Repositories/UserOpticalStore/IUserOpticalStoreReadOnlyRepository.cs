namespace GOtica.Domain.Repositories.UserOpticalStore;

public interface IUserOpticalStoreReadOnlyRepository
{
    Task<IReadOnlyCollection<Entities.UserOpticalStore>> GetUserOpticalStore(Guid userId);
    Task<bool> UserIsOwner(Guid userId);
    Task<bool> UserIsOwnerOfOpticalStore(Guid userId, Guid opticalId);
    Task<bool> UserBelongsToOptical(Guid userId, Guid opticalId);
    Task<string> GetUserOpticalRole(Guid userId, Guid opticalId);
}
