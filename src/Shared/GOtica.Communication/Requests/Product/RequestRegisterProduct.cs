using GOtica.Communication.Enums;

namespace GOtica.Communication.Requests.Product;

public record RequestRegisterProduct
{
    public string Name { get; init; } = string.Empty;
    public ProductType ProductType { get; init; }
    public string ProductCode { get; init; } = string.Empty;
    public decimal BasePrice { get; init; }
    public int StockQuantity { get; init; }
}
