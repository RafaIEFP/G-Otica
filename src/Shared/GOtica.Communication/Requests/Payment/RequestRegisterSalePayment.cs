using GOtica.Communication.Enums;

namespace GOtica.Communication.Requests.Payment;

public record RequestRegisterSalePayment
{
    public decimal Amount { get; init; }
    public PaymentMethod PaymentMethod { get; init; }
}
