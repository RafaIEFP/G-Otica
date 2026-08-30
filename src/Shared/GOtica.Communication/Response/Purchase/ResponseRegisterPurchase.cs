namespace GOtica.Communication.Response.Purchase;

public record ResponseRegisterPurchase
{
    public Guid Id { get; init; }
    public decimal TotalAmount { get; init; }
}
