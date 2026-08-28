namespace GOtica.Domain.Repositories.Supplier;

public interface ISupplierWriteOnlyRepository
{
    Task Add(Entities.Supplier supplier);
}
