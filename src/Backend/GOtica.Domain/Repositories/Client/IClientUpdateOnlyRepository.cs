namespace GOtica.Domain.Repositories.Client;

public interface IClientUpdateOnlyRepository
{
    Task<Entities.Client?> GetActiveInOpticalStore(Guid clientId, Guid opticalStoreId);
}
