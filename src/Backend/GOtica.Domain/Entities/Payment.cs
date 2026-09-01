using GOtica.Domain.Enums;

namespace GOtica.Domain.Entities;

public class Payment
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public decimal Amount { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
    public PaymentStatus Status { get; set; }
    public DateTime? ReceivedAt { get; set; }

    public Guid SaleId { get; set; }
    public Sale Sale { get; set; } = default!;

    public Guid? ReceivedByUserId { get; set; }
    public User? ReceivedByUser { get; set; }
}
