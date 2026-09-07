using GOtica.Domain.Enums;

namespace GOtica.Domain.Repositories.Sale;

public interface ISaleUpdateOnlyRepository
{
    Task<bool> TryUpdateStatus(
        Guid saleId,
        Guid opticalStoreId,
        SaleStatus expectedStatus,
        SaleStatus newStatus);
}
