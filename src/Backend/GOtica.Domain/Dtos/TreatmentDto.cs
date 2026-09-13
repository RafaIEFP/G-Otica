namespace GOtica.Domain.Dtos;

public class TreatmentDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal BasePrice { get; init; }
    public bool IsActive { get; init; }
}
