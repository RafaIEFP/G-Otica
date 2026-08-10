namespace GOtica.Domain.Repositories.UserOpticalStore;

public interface IUserOpticalStoreReadOnlyRepository
{
    Task<IReadOnlyCollection<Entities.UserOpticalStore>> GetUserOpticalStores(Guid userId);
}
