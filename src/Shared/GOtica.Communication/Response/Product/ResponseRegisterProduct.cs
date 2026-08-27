namespace GOtica.Communication.Response.Product;

public record ResponseRegisterProduct
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string ProductCode { get; init; } = string.Empty;
}
