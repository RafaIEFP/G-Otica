namespace GOtica.Domain.Repositories.Supplier;

public interface ISupplierUpdateOnlyRepository
{
    Task<Entities.Supplier?> GetById(Guid supplierId, Guid opticalStoreId);
}
