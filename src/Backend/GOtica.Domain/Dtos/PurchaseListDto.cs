namespace GOtica.Domain.Dtos;

public class PurchaseListDto
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public decimal TotalAmount { get; init; }
    public Guid SupplierId { get; init; }
    public string SupplierName { get; init; } = string.Empty;
    public Guid UserId { get; init; }
    public string UserName { get; init; } = string.Empty;
}
