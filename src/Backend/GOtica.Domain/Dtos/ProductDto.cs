using GOtica.Domain.Enums;

namespace GOtica.Domain.Dtos;

public class ProductDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public ProductType ProductType { get; init; }
    public string ProductCode { get; init; } = string.Empty;
    public decimal BasePrice { get; init; }
    public int StockQuantity { get; init; }
    public bool IsActive { get; init; }
}
