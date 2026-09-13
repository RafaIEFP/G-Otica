namespace GOtica.Domain.Entities;

public class ItemLensTreatment
{
    public Guid ItemLensId { get; set; }
    public ItemLens ItemLens { get; set; } = default!;

    public Guid TreatmentId { get; set; }
    public Treatment Treatment { get; set; } = default!;

    public decimal UnitPrice { get; set; }
}
