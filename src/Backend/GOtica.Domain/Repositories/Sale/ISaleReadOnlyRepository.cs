using GOtica.Domain.Dtos;

namespace GOtica.Domain.Repositories.Sale;

public interface ISaleReadOnlyRepository
{
    Task<SaleDto?> GetById(Guid saleId, Guid opticalStoreId);
}
