namespace GOtica.Domain.Repositories.Product;

public interface IProductUpdateOnlyRepository
{
    Task<Entities.Product?> GetActiveInOpticalStore(Guid productId,  Guid opticalStoreId);
}
