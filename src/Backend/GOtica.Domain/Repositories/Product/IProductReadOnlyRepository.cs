using GOtica.Domain.Dtos;

namespace GOtica.Domain.Repositories.Product;

public interface IProductReadOnlyRepository
{
    Task<bool> ProductAlreadyAtOpticalStore(string productCode, Guid opticalStoreId);
    Task<Entities.Product?> GetById(Guid productId, Guid opticalStoreId);
    Task<PagedResult<ProductDto>> GetAll(Guid opticalStoreId, int page, int pageSize, bool? isActive);
}
