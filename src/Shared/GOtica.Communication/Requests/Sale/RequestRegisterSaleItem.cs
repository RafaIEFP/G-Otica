namespace GOtica.Communication.Requests.Sale;

public record RequestRegisterSaleItem
{
    public Guid ProductId { get; init; }
    public int Quantity { get; init; }
    public decimal DiscountAmount { get; init; }
    public string? Notes { get; init; }
}
