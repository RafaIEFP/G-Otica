using GOtica.Domain.Dtos;

namespace GOtica.Domain.Repositories.UserOpticalStore;

public interface IUserOpticalStoreReadOnlyRepository
{
    Task<IReadOnlyCollection<Entities.UserOpticalStore>> GetAllUserOpticalStore(Guid userId);
    Task<IReadOnlyCollection<OpticalStoreUsersDto>> GetAllOpticalStoreUsers(Guid opticalId);
    Task<bool> UserIsOwner(Guid userId);
    Task<bool> UserIsOwnerOfOpticalStore(Guid userId, Guid opticalId);
    Task<bool> UserBelongsToOptical(Guid userId, Guid opticalId);
    Task<bool> UserBelongsToOpticalByEmail(string email, Guid opticalId);
    Task<Entities.UserOpticalStore?> GetUserOpticalStore(Guid userId, Guid opticalId);
    Task<OpticalStoreWithRoleDTO?> GetOpticalStoreWithRole(Guid userId, Guid opticalId);
    Task<IReadOnlyCollection<AllOpticalStoresWithRole>> GetAllOpticalStoresWithRole(Guid userId);
}
