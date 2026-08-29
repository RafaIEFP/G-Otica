namespace GOtica.Domain.Entities;

public class PurchaseItem
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalAmount { get; set; }

    public Guid PurchaseId { get; set; }
    public Purchase Purchase { get; set; } = default!;

    public Guid ProductId { get; set; }
    public Product Product { get; set; } = default!;
}
