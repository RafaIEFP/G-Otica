using GOtica.Communication.Requests.PurchaseItem;

namespace GOtica.Communication.Requests.Purchase;

public record RequestRegisterPurchase
{
    public Guid SupplierId { get; init; }

    public IReadOnlyCollection<RequestRegisterPurchaseItem> Items { get; init; } = [];
}