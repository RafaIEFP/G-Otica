using GOtica.Communication.Requests.User;
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

        var user = await _userUpdateOnlyRepository.GetUserById(loggedUser.Id);
        user.Password = _passwordEncryptor.Encrypt(request.NewPasswordConfirmed);

        _userUpdateOnlyRepository.Update(user);

        await _unitOfWork.Commit();
    }

    private void Validate(RequestChangePassword request, Domain.Entities.User loggedUser)
    {
        var validator = new ChangePasswordValidator().Validate(request);

        var passwordMatch = _passwordEncryptor.IsValid(request.Password, loggedUser.Password);

        if (!passwordMatch)
            validator.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesException.PASSWORD_DIFFERENT_CURRENT_ONE));

        if (request.NewPassword != request.NewPasswordConfirmed)
            validator.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesException.NEW_PASSWORDS_DO_NOT_MATCH));

        if (!validator.IsValid)
            throw new ErrorOnValidationException(validator.Errors.Select(e => e.ErrorMessage).ToList());
    }
}
