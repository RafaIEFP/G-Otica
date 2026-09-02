using GOtica.Communication.Requests.Payment;

namespace GOtica.Communication.Requests.Sale;

public record RequestRegisterSale
{
    public Guid ClientId { get; init; }
    public Guid? PrescriptionId { get; init; }
    public IReadOnlyCollection<RequestRegisterSaleItem> Items { get; init; } = [];
    public RequestRegisterSalePayment InitialPayment { get; init; } = new();
}
