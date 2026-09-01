using GOtica.Domain.Enums;

namespace GOtica.Domain.Entities;

public class Sale
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public SaleStatus Status { get; set; } = SaleStatus.Confirmed;
    public decimal TotalAmount { get; set; }

    public Guid OpticalStoreId { get; set; }
    public OpticalStore OpticalStore { get; set; } = default!;

    public Guid ClientId { get; set; }
    public Client Client { get; set; } = default!;

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public Guid? PrescriptionId { get; set; }
    public Prescription? Prescription { get; set; }

    public ICollection<SaleItem> Items { get; set; } = [];

    // public ICollection<Payment> Payments { get; set; } = [];
}
