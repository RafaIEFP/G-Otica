using GOtica.Domain.Dtos;

namespace GOtica.Domain.Repositories.UserOpticalStore;

public interface IUserOpticalStoreReadOnlyRepository
{
    Task<IReadOnlyCollection<Entities.UserOpticalStore>> GetUserOpticalStore(Guid userId);
    Task<bool> UserIsOwner(Guid userId);
    Task<bool> UserIsOwnerOfOpticalStore(Guid userId, Guid opticalId);
    Task<bool> UserBelongsToOptical(Guid userId, Guid opticalId);
    Task<string> GetUserOpticalRole(Guid userId, Guid opticalId);
    Task<OpticalStoreWithRoleDTO?> GetOpticalStoreWithRole(Guid userId, Guid opticalId);
    Task<IReadOnlyCollection<AllOpticalStoresWithRole>> GetAllOpticalStoresWithRole(Guid userId);
}
