namespace GOtica.Communication.Response.OpticalStore;

public record ResponseRegisterOpticalStore
{
    public Guid Id { get; init; } 
    public string Name { get; init; } = string.Empty;
}
