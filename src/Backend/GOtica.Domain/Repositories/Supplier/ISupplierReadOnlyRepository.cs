using GOtica.Domain.Dtos;

namespace GOtica.Domain.Repositories.Supplier;

public interface ISupplierReadOnlyRepository
{
    Task<Entities.Supplier?> GetById(Guid supplierId, Guid opticalStoreId);
    Task<PagedResult<SupplierDto>> GetAll(
        Guid opticalStoreId,
        int page,
        int pageSize,
        bool? isActive);
}
