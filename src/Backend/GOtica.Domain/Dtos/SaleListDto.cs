using GOtica.Domain.Enums;

namespace GOtica.Domain.Dtos;

public class SaleListDto
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public SaleStatus Status { get; init; }
    public decimal TotalAmount { get; init; }
    public decimal ReceivedAmount { get; init; }
    public Guid ClientId { get; init; }
    public string ClientName { get; init; } = string.Empty;
}
