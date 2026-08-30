namespace GOtica.Domain.Dtos;

public class PurchaseItemDto
{
    public Guid Id { get; init; }

    public Guid ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public string ProductCode { get; init; } = string.Empty;

    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public decimal TotalAmount { get; init; }
}
