namespace GOtica.Domain.Repositories.Supplier;

public interface ISupplierUpdateOnlyRepository
{
    Task<Entities.Supplier?> GetById(Guid supplierId, Guid opticalStoreId);
    Task<bool> Deactivate(Guid supplierId, Guid opticalStoreId);
    Task<bool> Reactivate(Guid supplierId, Guid opticalStoreId);
}
