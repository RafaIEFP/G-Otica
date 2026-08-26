using GOtica.Domain.Enums;

namespace GOtica.Domain.Entities;

public class Product
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public string Name { get; set; } = string.Empty;
    public ProductType ProductType { get; set; }
    public string ProductCode { get; set; } = string.Empty;
    public decimal BasePrice { get; set; }
    public int StockQuantity { get; set; }

    public bool IsActive { get; set; } = true;

    public Guid OpticalStoreId { get; set; }
    public OpticalStore OpticalStore { get; set; } = default!;
}
