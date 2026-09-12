using GOtica.Domain.Enums;

namespace GOtica.Domain.Dtos;

public class SaleCancellationDto
{
    public SaleStatus Status { get; init; }
    public IReadOnlyCollection<SaleCancellationItemDto> Items { get; init; } = [];
}
