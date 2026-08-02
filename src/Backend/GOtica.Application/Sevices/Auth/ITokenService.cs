using GOtica.Domain.Dtos;
using GOtica.Domain.Entities;

namespace GOtica.Application.Sevices.Auth;

public interface ITokenService
{
    TokensDto GenerateTokens(User user);
}
