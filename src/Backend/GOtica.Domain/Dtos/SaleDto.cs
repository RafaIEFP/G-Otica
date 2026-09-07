using GOtica.Domain.Entities;
using GOtica.Domain.Enums;

namespace GOtica.Domain.Dtos;

public class SaleDto
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public SaleStatus Status { get; init; }
    public decimal TotalAmount { get; init; }
    public Guid ClientId { get; init; }
    public string ClientName { get; init; } = string.Empty;
    public Guid UserId { get; init; }
    public string UserName { get; init; } = string.Empty;
    public Guid? PrescriptionId { get; init; }
    public IReadOnlyCollection<SaleItemDto> Items { get; init; } = [];
    public IReadOnlyCollection<PaymentDto> Payments { get; init; } = [];
}
