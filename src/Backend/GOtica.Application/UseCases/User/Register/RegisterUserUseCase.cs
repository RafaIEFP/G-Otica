using FluentValidation.Results;
using GOtica.Application.Sevices.Auth;
using GOtica.Communication.Requests.User;
using GOtica.Communication.Response;
using GOtica.Communication.Response.User;
using GOtica.Domain.Entities;
using GOtica.Domain.Repositories;
using GOtica.Domain.Repositories.Refresh;
using GOtica.Domain.Repositories.User;
using GOtica.Domain.Security.Cryptography;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;
using Mapster;
using Microsoft.Extensions.Options;

namespace GOtica.Application.UseCases.User.Register;

public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
    private readonly IPasswordEncryptor _passwordEncriptor;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenWriteOnlyRepository _refreshTokenRepository;
    private readonly TokenSettings _tokenSettings;

    public RegisterUserUseCase(
        IUnitOfWork unitOfWork,
        IUserWriteOnlyRepository userWriteOnlyRepository,
        IUserReadOnlyRepository userReadOnlyRepository,
        IPasswordEncryptor passwordEncriptor,
        ITokenService tokenService,
        IRefreshTokenWriteOnlyRepository refreshTokenRepository,
        IOptions<TokenSettings> tokenSettings)
    {
        _unitOfWork = unitOfWork;
        _userReadOnlyRepository = userReadOnlyRepository;
        _userWriteOnlyRepository = userWriteOnlyRepository;
        _passwordEncriptor = passwordEncriptor;
        _tokenService = tokenService;
        _refreshTokenRepository = refreshTokenRepository;
        _tokenSettings = tokenSettings.Value;
    }

    public async Task<ResponseRegisteredUser> Execute(RequestRegisterUser request)
    {
        await Validate(request);

        var user = request.Adapt<Domain.Entities.User>();
        user.Password = _passwordEncriptor.Encrypt(request.Password);

        var tokens = _tokenService.GenerateTokens(user);

        await _userWriteOnlyRepository.Add(user);

        await _refreshTokenRepository.Add(new RefreshToken
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

    private async Task Validate(RequestRegisterUser request)
    {
        var result = new RegisterUserValidator().Validate(request);

        var emailExist = await _userReadOnlyRepository.ExistUserWithEmail(request.Email);
        if (emailExist)
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.EMAIL_ALREADY_REGISTERED));

        if (!result.IsValid)
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }
}
