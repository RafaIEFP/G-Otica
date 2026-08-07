namespace GOtica.Communication.Requests;

public record RequestChangePassword
{
    public string Password { get; init; } = string.Empty;
    public string NewPassword { get; init; } = string.Empty;
    public string NewPasswordConfirmed { get; init; } = string.Empty;
}
