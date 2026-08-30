namespace GOtica.Communication.Requests.PurchaseItem;

public record RequestRegisterPurchaseItem
{
    public Guid ProductId { get; init; }
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
}
