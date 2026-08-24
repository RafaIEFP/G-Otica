using GOtica.Application.Sevices.Auth;
using GOtica.Communication.Requests;
using GOtica.Communication.Response.User;
using GOtica.Domain.Entities;
using GOtica.Domain.Repositories;
using GOtica.Domain.Repositories.Refresh;
using GOtica.Domain.Repositories.User;
using GOtica.Domain.Security.Cryptography;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;
using Microsoft.Extensions.Options;

namespace GOtica.Application.UseCases.User.Reactivate;

public class ReactivateUserUseCase : IReactivateUserUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IPasswordEncryptor _passwordEncryptor;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenWriteOnlyRepository _refreshTokenRepository;
    private readonly TokenSettings _tokenSettings;

    public ReactivateUserUseCase(
        IUnitOfWork unitOfWork,
        IUserUpdateOnlyRepository userUpdateOnlyRepository,
        IUserReadOnlyRepository userReadOnlyRepository,
        IPasswordEncryptor passwordEncryptor,
        ITokenService tokenService,
        IRefreshTokenWriteOnlyRepository refreshTokenRepository,
        IOptions<TokenSettings> tokenSettings)
    {
        _unitOfWork = unitOfWork;
        _userUpdateOnlyRepository = userUpdateOnlyRepository;
        _userReadOnlyRepository = userReadOnlyRepository;
        _passwordEncryptor = passwordEncryptor;
        _tokenService = tokenService;
        _refreshTokenRepository = refreshTokenRepository;
        _tokenSettings = tokenSettings.Value;
    }

    public async Task<ResponseRegisteredUser> Execute(RequestReactivateUser request)
    {
        Validate(request);

        var user = await _userReadOnlyRepository.GetUserByEmail(request.Email)
            ??
            throw new UnauthorizedException(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID);

        var passwordIsValid = _passwordEncryptor.IsValid(request.Password, user.Password);

        if (!passwordIsValid)
            throw new UnauthorizedException(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID);

        if (user.IsActive)
            throw new ConflictException(ResourceMessagesException.USER_ACCOUNT_ALREADY_ACTIVE);

        var tokens = _tokenService.GenerateTokens(user);

        var refreshToken = new RefreshToken
        {
            UserId = user.Id,
            Token = tokens.Refresh,
            AccessTokenId = tokens.AccessTokenId,
            ExpiresAt = DateTime.UtcNow.AddDays(_tokenSettings.RefreshTokenValidityDays)
        };

        await _unitOfWork.ExecuteInTransaction(async () =>
        {
            await _userUpdateOnlyRepository.ReactivateAccount(user.Id);

            await _refreshTokenRepository.Add(refreshToken);
        });

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

    private void Validate(RequestReactivateUser request)
    {
        var result = new ReactivateUserValidator().Validate(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }
}
