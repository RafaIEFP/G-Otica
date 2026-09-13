namespace GOtica.Communication.Requests.Treatment;

public record RequestRegisterTreatment
{
    public string Name { get; init; } = string.Empty;
    public decimal BasePrice { get; init; }
}
