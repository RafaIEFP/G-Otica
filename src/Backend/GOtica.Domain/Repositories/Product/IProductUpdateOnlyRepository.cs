namespace GOtica.Domain.Repositories.Product;

public interface IProductUpdateOnlyRepository
{
    Task<Entities.Product?> GetActiveInOpticalStore(Guid productId,  Guid opticalStoreId);
    Task<bool> Deactivate(Guid productId, Guid opticalStoreId);
    Task<bool> Reactivate(Guid productId, Guid opticalStoreId);
    Task<bool> AdjustStock(Guid productId, Guid opticalStoreId, int quantityChange);
    Task<bool> TryDecreaseStock(Guid productId, Guid opticalStoreId, int quantity);
}
