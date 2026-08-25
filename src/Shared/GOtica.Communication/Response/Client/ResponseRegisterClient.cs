namespace GOtica.Communication.Response.Client;

public record ResponseRegisterClient
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
}
