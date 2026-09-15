namespace GOtica.Domain.Dtos;

public class ItemLensTreatmentDto
{
    public Guid TreatmentId { get; init; }
    public string TreatmentName { get; init; } = string.Empty;
    public decimal UnitPrice { get; init; }
}
