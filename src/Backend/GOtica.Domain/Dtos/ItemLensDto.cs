using GOtica.Domain.Enums;

namespace GOtica.Domain.Dtos;

public class ItemLensDto
{
    public Guid Id { get; init; }
    public EyeSide EyeSide { get; init; }
    public decimal? PupillaryDistance { get; init; }
    public decimal? NasoPupillaryDistance { get; init; }
    public LensType LensType { get; init; }
    public decimal RefractiveIndex { get; init; }
    public LensMaterial Material { get; init; }
    public string? Color { get; init; }
    public decimal Diameter { get; init; }
    public IReadOnlyCollection<ItemLensTreatmentDto> Treatments { get; init; } = [];
}
