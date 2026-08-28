using GOtica.Domain.Dtos;

namespace GOtica.Domain.Repositories.StockMovement;

public interface IStockMovementReadOnlyRepository
{
    Task<PagedResult<StockMovementDto>> GetAll(Guid productId, int page, int pageSize);
}
