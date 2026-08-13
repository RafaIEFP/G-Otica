namespace GOtica.Domain.Repositories.User;

public interface IUserReadOnlyRepository
{
    Task<bool> ExistUserWithEmail(string email);
    Task<Entities.User?> GetUserByEmail(string email);
    Task<Entities.User?> GetUserById(Guid id);
    Task<bool> ExistsActiveUser(Guid id);
}
