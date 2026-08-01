namespace GOtica.Communication.Response;

public class ResponseRegisterUser
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ResponseTokens Tokens { get; set; } = default!;
}
