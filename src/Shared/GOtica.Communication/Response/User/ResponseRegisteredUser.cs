namespace GOtica.Communication.Response.User;

public record ResponseRegisteredUser
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty; 
    public ResponseTokens Tokens { get; init; } = default!;
}
