using GOtica.Application.Sevices.Auth;
using GOtica.Communication.Requests;
using GOtica.Communication.Response;
using GOtica.Domain.Repositories;
using GOtica.Domain.Repositories.Refresh;
using GOtica.Domain.Repositories.User;
using GOtica.Domain.Security.Cryptography;
using GOtica.Exceptions.ExceptionsBase;

namespace GOtica.Application.UseCases.Login.DoLogin;

public class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IPasswordEncryptor _passwordEncryptor;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenWriteOnlyRepository _refreshTokenRepository;

    public DoLoginUseCase(
        IUnitOfWork unitOfWork,
        IUserReadOnlyRepository userReadOnlyRepository,
        IPasswordEncryptor passwordEncriptor,
        ITokenService tokenService,
        IRefreshTokenWriteOnlyRepository refreshTokenRepository)
    {
        _unitOfWork = unitOfWork;
        _userReadOnlyRepository = userReadOnlyRepository;
        _passwordEncryptor = passwordEncriptor;
        _tokenService = tokenService;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<ResponseRegisteredUser> Execute(RequestLogin request)
    {
        var user = await _userReadOnlyRepository.GetUserByEmail(request.Email) 
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
            ExpiresAt = DateTime.UtcNow.AddDays(7)
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
