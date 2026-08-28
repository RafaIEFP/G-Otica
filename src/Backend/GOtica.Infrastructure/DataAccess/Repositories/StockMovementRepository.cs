using GOtica.Domain.Dtos;
using GOtica.Domain.Entities;
using GOtica.Domain.Repositories;
using GOtica.Domain.Repositories.StockMovement;
using Microsoft.EntityFrameworkCore;

namespace GOtica.Infrastructure.DataAccess.Repositories;

internal sealed class StockMovementRepository(GOticaDbContext dbContext) : IStockMovementWriteOnlyRepository, IStockMovementReadOnlyRepository
{
    public async Task Add(StockMovement stockMovement)
    {
        await dbContext.StockMovements.AddAsync(stockMovement);
    }

    public async Task<PagedResult<StockMovementDto>> GetAll(Guid productId, int page, int pageSize)
    {
        var query = dbContext.StockMovements.AsNoTracking().Where(m => m.ProductId == productId);

        var totalCount = await query.CountAsync();

        var movements = await query
            .OrderByDescending(m => m.CreatedAt)
            .ThenByDescending(m => m.Id)
            .Paged(page, pageSize)
            .Select(m => new StockMovementDto
            {
                Id = m.Id,
                QuantityChange = m.QuantityChange,
                Type = m.Type,
                Reason = m.Reason,
                CreatedAt = m.CreatedAt,

                UserId = m.UserId,
                UserName = m.User.Name
            })
            .ToListAsync();

        return new PagedResult<StockMovementDto>
        {
            Items = movements,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}
