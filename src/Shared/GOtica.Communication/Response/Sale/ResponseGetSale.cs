using GOtica.Communication.Enums;

namespace GOtica.Communication.Response.Sale;

public record ResponseGetSale
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public SaleStatus Status { get; init; }
    public decimal TotalAmount { get; init; }
    public decimal ReceivedAmount { get; init; }
    public decimal RemainingAmount { get; init; }
    public ResponseGetSaleClient Client { get; init; } = default!;
    public ResponseGetSaleUser RegisteredBy { get; init; } = default!;
    public Guid? PrescriptionId { get; init; }
    public IReadOnlyCollection<ResponseGetSaleItem> Items { get; init; } = [];

    public IReadOnlyCollection<ResponseGetSalePayment> Payments { get; init; } = [];
}
