namespace GOtica.Communication.Response.Supplier;

public record ResponseRegisterSupplier
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
}