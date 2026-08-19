using GOtica.Application.Sevices.Auth;
using GOtica.Communication.Requests;
using GOtica.Communication.Response.User;
using GOtica.Domain.Repositories;
using GOtica.Domain.Repositories.Refresh;
using GOtica.Domain.Security.Tokens.Access;
using GOtica.Exceptions.ExceptionsBase;
using Microsoft.Extensions.Options;

namespace GOtica.Application.UseCases.Token.RefreshToken;

public class RefreshTokenUseCase : IRefreshTokenUseCase
{
    private readonly IRefreshTokenReadOnlyRepository _refreshTokenReadOnlyRepository;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenWriteOnlyRepository _refreshTokenWriteOnlyRepository;
    private readonly IAccessTokenValidator _accessTokenValidator;
    private readonly TokenSettings _tokenSettings;
    private readonly IUnitOfWork _unitOfWork;
    public RefreshTokenUseCase(
        IRefreshTokenReadOnlyRepository refreshTokenReadOnlyRepository,
        ITokenService tokenService,
        IRefreshTokenWriteOnlyRepository refreshTokenWriteOnlyRepository,
        IAccessTokenValidator accessTokenValidator,
        IOptions<TokenSettings> tokenSettings,
        IUnitOfWork unitOfWork)
    {
        _refreshTokenReadOnlyRepository = refreshTokenReadOnlyRepository;
        _tokenService = tokenService;
        _refreshTokenWriteOnlyRepository = refreshTokenWriteOnlyRepository;
        _accessTokenValidator = accessTokenValidator;
        _tokenSettings = tokenSettings.Value;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseTokens> Execute(RequestNewToken request)
    {
        var refreshToken = await _refreshTokenReadOnlyRepository.Get(request.RefreshToken)
            ?? throw new RefreshTokenNotFoundException();

        var accessTokenIdentifier = _accessTokenValidator.GetAccessTokenIdentifier(request.AccessToken);

        if (accessTokenIdentifier != refreshToken.AccessTokenId)
            throw new RefreshTokenNotFoundException();

        var hasTokenAssociated = await _refreshTokenReadOnlyRepository.HasRefresTokenAssociated(refreshToken.UserId, accessTokenIdentifier);

        if (hasTokenAssociated == false)
            throw new RefreshTokenNotFoundException();

        if (refreshToken.IsExpired)
            throw new RefreshTokenExpiredException();

        var tokens = _tokenService.GenerateTokens(refreshToken.User);

        await _refreshTokenWriteOnlyRepository.Add(new Domain.Entities.RefreshToken
        {
            UserId = refreshToken.UserId,
            Token = tokens.Refresh,
            AccessTokenId = tokens.AccessTokenId,
            ExpiresAt = DateTime.UtcNow.AddDays(_tokenSettings.RefreshTokenValidityDays)
        });

        await _unitOfWork.Commit();

        return new ResponseTokens
        {
            AccessToken = tokens.Access,
            RefreshToken = tokens.Refresh
        };
    }
}
