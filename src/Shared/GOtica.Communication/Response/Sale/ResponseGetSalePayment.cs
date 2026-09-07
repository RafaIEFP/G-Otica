using GOtica.Communication.Enums;

namespace GOtica.Communication.Response.Sale;

public record ResponseGetSalePayment
{
    public Guid Id { get; init; }
    public decimal Amount { get; init; }
    public PaymentStatus Status { get; init; }
    public PaymentMethod? PaymentMethod { get; init; }
    public DateTime? ReceivedAt { get; init; }
    public ResponseGetSaleUser? ReceivedBy { get; init; }
}
