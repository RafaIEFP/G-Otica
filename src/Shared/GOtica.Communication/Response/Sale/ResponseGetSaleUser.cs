namespace GOtica.Communication.Response.Sale;

public record ResponseGetSaleUser
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
}
