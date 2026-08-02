using FluentValidation.Results;
using GOtica.Application.Sevices.Auth;
using GOtica.Communication.Requests;
using GOtica.Communication.Response;
using GOtica.Domain.Entities;
using GOtica.Domain.Repositories;
using GOtica.Domain.Repositories.Refresh;
using GOtica.Domain.Repositories.User;
using GOtica.Domain.Security.Cryptography;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;
using Mapster;
using MapsterMapper;

namespace GOtica.Application.UseCases.User.Register;

public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
    private readonly IPasswordEncryptor _passwordEncriptor;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenWriteOnlyRepository _refreshTokenRepository;

    public RegisterUserUseCase(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IUserWriteOnlyRepository userWriteOnlyRepository,
        IUserReadOnlyRepository userReadOnlyRepository,
        IPasswordEncryptor passwordEncriptor,
        ITokenService tokenService,
        IRefreshTokenWriteOnlyRepository refreshTokenRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userReadOnlyRepository = userReadOnlyRepository;
        _userWriteOnlyRepository = userWriteOnlyRepository;
        _passwordEncriptor = passwordEncriptor;
        _tokenService = tokenService;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<ResponseRegisterUser> Execute(RequestRegisterUser request)
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
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        });

        await _unitOfWork.Commit();

        return new ResponseRegisterUser
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

        var emailExist = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);
        if (emailExist)
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.EMAIL_ALREADY_REGISTERED));

        if (!result.IsValid)
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }
}
