using GOtica.Communication.Enums;

namespace GOtica.Communication.Requests.Product;

public record RequestAdjustProductStock
{
    public StockAdjustmentType Type { get; init; }
    public int Quantity { get; init; }
    public string Reason { get; init; } = string.Empty;
}
