namespace GOtica.Domain.Repositories.Product;

public interface IProductReadOnlyRepository
{
    Task<bool> ProductAlreadyAtOpticalStore(string productCode, Guid opticalStoreId);
    Task<Entities.Product?> GetById(Guid productId, Guid opticalStoreId);
}
