namespace GOtica.Communication.Response.Treatment;

public record ResponseRegisterTreatment
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal BasePrice { get; init; }
}
