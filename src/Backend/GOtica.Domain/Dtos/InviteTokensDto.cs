namespace GOtica.Domain.Dtos;

public record InviteTokensDto
{
    public string Token { get; init; } = string.Empty;
    public string TokenHash { get; init; } = string.Empty;
}
