using GOtica.Communication.Enums;

namespace GOtica.Communication.Requests.Payment;

public record RequestReceivePayment
{
    public PaymentMethod? PaymentMethod { get; init; }
}
