using GOtica.Communication.Enums;

namespace GOtica.Communication.Response.Sale;

public record RequestGetAllSales
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public SaleStatus? Status { get; init; }
}
