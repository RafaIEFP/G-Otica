namespace GOtica.Domain.Repositories.User;

public interface IUserReadOnlyRepository
{
    Task<bool> ExistUserWithEmail(string email);
    Task<Entities.User?> GetActiveUserByEmail(string email);
    Task<Entities.User?> GetUserByEmail(string email);
    Task<Entities.User?> GetUserById(Guid id);
    Task<bool> ExistsActivatedUser(Guid id);
}
