using GOtica.Communication.Enums;

namespace GOtica.Communication.Response.StockMovement;

public record ResponseStockMovement
{
    public Guid Id { get; init; }
    public int QuantityChange { get; init; }
    public StockMovementType Type { get; init; }
    public string? Reason { get; init; }
    public DateTime CreatedAt { get; init; }

    public Guid UserId { get; init; }
    public string UserName { get; init; } = string.Empty;
}
