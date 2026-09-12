using GOtica.Domain.Dtos;
using GOtica.Domain.Enums;

namespace GOtica.Domain.Repositories.Sale;

public interface ISaleReadOnlyRepository
{
    Task<SaleDto?> GetById(Guid saleId, Guid opticalStoreId);
    Task<PagedResult<SaleListDto>> GetAll(Guid opticalStoreId, int page, int pageSize, SaleStatus? status);
    Task<bool> Exist(Guid saleId, Guid opticalStoreId);
    Task<SaleDeliveryDto?> GetDeliveryData(Guid saleId, Guid opticalStoreId);
    Task<SaleCancellationDto?> GetCancellationData(Guid saleId, Guid opticalStoreId);
}
