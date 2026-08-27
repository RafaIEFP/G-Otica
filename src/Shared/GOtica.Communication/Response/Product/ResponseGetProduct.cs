using GOtica.Communication.Enums;

namespace GOtica.Communication.Response.Product;

public record ResponseGetProduct
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public ProductType ProductType { get; init; }
    public string ProductCode { get; init; } = string.Empty;
    public decimal BasePrice { get; init; }
    public int StockQuantity { get; init; }
    public bool IsActive { get; init; }
}
