using GOtica.Domain.Enums;

namespace GOtica.Domain.Entities;

public class ItemLens
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public EyeSide EyeSide { get; set; }
    public decimal? PupillaryDistance { get; set; }
    public decimal? NasoPupillaryDistance { get; set; }
    public LensType LensType { get; set; }
    public decimal RefractiveIndex { get; set; }
    public LensMaterial Material { get; set; }
    public string? Color { get; set; }
    public decimal? Diameter { get; set; }

    public Guid SaleItemId { get; set; }
    public SaleItem SaleItem { get; set; } = default!;

    public ICollection<ItemLensTreatment> Treatments { get; set; } = [];
}
