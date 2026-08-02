namespace GOtica.Communication.Response;

public record ResponseRegisterUser
{
    public long Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public ResponseTokens Tokens { get; init; } = default!;
}
