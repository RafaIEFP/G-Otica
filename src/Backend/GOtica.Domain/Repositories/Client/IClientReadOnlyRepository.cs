namespace GOtica.Domain.Repositories.Client;

public interface IClientReadOnlyRepository
{
    Task<Entities.Client?> Get(Guid clientId, Guid opticalStoreId);
}
