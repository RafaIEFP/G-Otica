using GOtica.Domain.Enums;

namespace GOtica.Domain.Dtos;

public class SaleDeliveryDto
{
    public SaleStatus Status { get; init; }
    public decimal TotalAmount { get; init; }
    public decimal ReceivedAmount { get; init; }
}
