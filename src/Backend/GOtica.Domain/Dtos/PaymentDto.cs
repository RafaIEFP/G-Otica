using GOtica.Domain.Enums;

namespace GOtica.Domain.Dtos;

public class PaymentDto
{
    public Guid Id { get; init; }
    public decimal Amount { get; init; }
    public PaymentStatus Status { get; init; }
    public PaymentMethod? PaymentMethod { get; init; }
    public DateTime? ReceivedAt { get; init; }
    public Guid? ReceivedByUserId { get; init; }
    public string? ReceivedByUserName { get; init; }
}
