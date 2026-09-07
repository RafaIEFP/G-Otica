using GOtica.Communication.Enums;

namespace GOtica.Communication.Response.Sale;

public record ResponseGetAllSales
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public SaleStatus Status { get; init; }
    public decimal TotalAmount { get; init; }
    public decimal ReceivedAmount { get; init; }
    public decimal RemainingAmount { get; init; }
    public Guid ClientId { get; init; }
    public string ClientName { get; init; } = string.Empty;
}
