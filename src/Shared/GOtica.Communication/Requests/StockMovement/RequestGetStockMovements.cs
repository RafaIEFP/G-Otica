namespace GOtica.Communication.Requests.StockMovement;

public record RequestGetStockMovements
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}
