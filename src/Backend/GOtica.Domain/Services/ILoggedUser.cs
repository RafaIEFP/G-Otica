namespace GOtica.Domain.Services;

public interface ILoggedUser
{
    Task<Entities.User> Get();
}
