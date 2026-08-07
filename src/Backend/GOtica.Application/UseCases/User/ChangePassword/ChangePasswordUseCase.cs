using GOtica.Communication.Requests;
using GOtica.Domain.Entities;
using GOtica.Domain.Repositories;
using GOtica.Domain.Repositories.User;
using GOtica.Domain.Security.Cryptography;
using GOtica.Domain.Services;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.User.ChangePassword;

public class ChangePasswordUseCase : IChangePasswordUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository;
    private readonly IPasswordEncryptor _passwordEncryptor;
    private readonly IUnitOfWork _unitOfWork;

    public ChangePasswordUseCase(
    ILoggedUser loggedUser,
    IUserUpdateOnlyRepository userUpdateOnlyRepository,
    IPasswordEncryptor passwordEncryptor,
    IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _userUpdateOnlyRepository = userUpdateOnlyRepository;
        _passwordEncryptor = passwordEncryptor;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(RequestChangePassword request)
    {
        var loggedUser = await _loggedUser.Get();

        Validate(request, loggedUser);
    }

    private void Validate(RequestChangePassword request, Domain.Entities.User loggedUser)
    {
        var validator = new ChangePasswordValidator().Validate(request);

        var passwordMatch = _passwordEncryptor.IsValid(request.Password, loggedUser.Password);

        if (!passwordMatch)
            validator.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesException.PASSWORD_DIFFERENT_CURRENT_ONE));

        if (!validator.IsValid)
            throw new ErrorOnValidationException(validator.Errors.Select(e => e.ErrorMessage).ToList());
    }
}
