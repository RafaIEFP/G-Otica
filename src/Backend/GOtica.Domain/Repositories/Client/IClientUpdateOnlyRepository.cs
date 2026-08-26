namespace GOtica.Domain.Repositories.Client;

public interface IClientUpdateOnlyRepository
{
    Task<Entities.Client?> GetActiveInOpticalStore(Guid clientId, Guid opticalStoreId);
    Task<bool> Deactivate(Guid clientId, Guid opticalStoreId);
}
