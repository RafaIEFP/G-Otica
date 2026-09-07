namespace GOtica.Communication.Response.Sale;

public record ResponseGetSaleClient
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
}
