using GOtica.Domain.Dtos;

namespace GOtica.Domain.Repositories.Purchase;

public interface IPurchaseReadOnlyRepository
{
    Task<PurchaseDto?> GetById(Guid purchaseId, Guid opticalStoreId);
}
