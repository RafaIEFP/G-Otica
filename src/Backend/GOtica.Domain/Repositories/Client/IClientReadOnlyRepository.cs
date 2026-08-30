using GOtica.Domain.Dtos;

namespace GOtica.Domain.Repositories.Client;

public interface IClientReadOnlyRepository
{
    Task<Entities.Client?> Get(Guid clientId, Guid opticalStoreId);
    Task<PagedResult<ClientDto>> GetAll(Guid opticalStoreId, int page, int pageSize, bool? isActive);
    Task<bool> ExistActive(Guid clientId, Guid opticalStoreId);
    Task<bool> Exist(Guid clientId, Guid opticalStoreId);
}
