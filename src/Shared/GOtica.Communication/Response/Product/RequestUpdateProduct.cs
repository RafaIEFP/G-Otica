using GOtica.Communication.Enums;

namespace GOtica.Communication.Response.Product;

public record RequestUpdateProduct
{
    public string Name { get; init; } = string.Empty;
    public ProductType ProductType { get; init; }
    public string ProductCode { get; init; } = string.Empty;
    public decimal BasePrice { get; init; }
}
