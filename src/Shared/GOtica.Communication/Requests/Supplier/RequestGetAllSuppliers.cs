namespace GOtica.Communication.Requests.Supplier;

public record RequestGetAllSuppliers
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public bool? IsActive { get; init; }
}
