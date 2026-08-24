using GOtica.Application.Sevices.Auth;
using GOtica.Communication.Requests.User;
using GOtica.Communication.Response;
using GOtica.Communication.Response.User;
using GOtica.Domain.Repositories;
using GOtica.Domain.Repositories.Refresh;
using GOtica.Domain.Repositories.User;
using GOtica.Domain.Security.Cryptography;
using GOtica.Exceptions.ExceptionsBase;
using Microsoft.Extensions.Options;

namespace GOtica.Application.UseCases.Login.DoLogin;

public class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IPasswordEncryptor _passwordEncryptor;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenWriteOnlyRepository _refreshTokenRepository;
    private readonly TokenSettings _tokenSettings;

    public DoLoginUseCase(
        IUnitOfWork unitOfWork,
        IUserReadOnlyRepository userReadOnlyRepository,
        IPasswordEncryptor passwordEncriptor,
        ITokenService tokenService,
        IRefreshTokenWriteOnlyRepository refreshTokenRepository,
        IOptions<TokenSettings> tokenSettings)
    {
        _unitOfWork = unitOfWork;
        _userReadOnlyRepository = userReadOnlyRepository;
        _passwordEncryptor = passwordEncriptor;
        _tokenService = tokenService;
        _refreshTokenRepository = refreshTokenRepository;
        _tokenSettings = tokenSettings.Value;
    }

    public async Task<ResponseRegisteredUser> Execute(RequestLogin request)
    {
        var user = await _userReadOnlyRepository.GetActiveUserByEmail(request.Email) 
            ?? throw new InvalidLoginException();

        var passwordMatch = _passwordEncryptor.IsValid(request.Password, user.Password);

        if (!passwordMatch)
            throw new InvalidLoginException();

        var tokens = _tokenService.GenerateTokens(user);

        await _refreshTokenRepository.Add(new Domain.Entities.RefreshToken
        {
            UserId = user.Id,
            Token = tokens.Refresh,
            AccessTokenId = tokens.AccessTokenId,
            ExpiresAt = DateTime.UtcNow.AddDays(_tokenSettings.RefreshTokenValidityDays)
        });

        await _unitOfWork.Commit();

        return new ResponseRegisteredUser
        {
            Id = user.Id,
            Name = user.Name,
            Tokens = new ResponseTokens
            {
                AccessToken = tokens.Access,
                RefreshToken = tokens.Refresh
            }
        };
    }
}
