using GOtica.Communication.Enums;

namespace GOtica.Communication.Response.Sale;

public record ResponseRegisterSale
{
    public Guid Id { get; init; }
    public decimal TotalAmount { get; init; }
    public SaleStatus Status { get; init; }
    public DateTime CreatedAt { get; init; }
}
