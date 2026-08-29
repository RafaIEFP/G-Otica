namespace GOtica.Domain.Entities;

public class Purchase
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public decimal TotalAmount { get; set; }

    public Guid SupplierId { get; set; }
    public Supplier Supplier { get; set; } = default!;

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public Guid OpticalStoreId { get; set; }
    public OpticalStore OpticalStore { get; set; } = default!;

    public ICollection<PurchaseItem> Items { get; set; } = [];
}
