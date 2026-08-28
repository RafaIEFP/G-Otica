namespace GOtica.Domain.Repositories.Supplier;

public interface ISupplierReadOnlyRepository
{
    Task<Entities.Supplier?> GetById(Guid supplierId, Guid opticalStoreId);
}
