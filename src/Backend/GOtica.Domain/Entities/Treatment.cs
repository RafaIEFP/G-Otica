namespace GOtica.Domain.Entities;

public class Treatment
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Name { get; set; } = string.Empty;
    public decimal BasePrice { get; set; }
    public bool IsActive { get; set; } = true;

    public Guid OpticalStoreId { get; set; }
    public OpticalStore OpticalStore { get; set; } = default!;

    public ICollection<ItemLensTreatment> ItemLensTreatments { get; set; } = [];
}
