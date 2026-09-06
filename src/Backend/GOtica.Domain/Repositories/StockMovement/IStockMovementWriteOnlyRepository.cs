namespace GOtica.Domain.Repositories.StockMovement;

public interface IStockMovementWriteOnlyRepository
{
    Task Add(Entities.StockMovement stockMovement);
    Task AddRange(IReadOnlyCollection<Entities.StockMovement> stockMovements);
}
