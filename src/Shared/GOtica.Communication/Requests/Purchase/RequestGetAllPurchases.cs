namespace GOtica.Communication.Requests.Purchase;

public record RequestGetAllPurchases
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}
