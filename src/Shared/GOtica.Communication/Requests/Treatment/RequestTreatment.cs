namespace GOtica.Communication.Requests.Treatment;

public record RequestTreatment
{
    public string Name { get; init; } = string.Empty;
    public decimal BasePrice { get; init; }
}
