namespace GOtica.Communication.Requests.Product;

public record RequestGetAllProducts
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public bool? IsActive { get; init; }
}
