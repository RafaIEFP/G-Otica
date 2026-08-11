namespace GOtica.Domain.Repositories.User;

public interface IUserReadOnlyRepository
{
    Task<bool> ExistActiveUserWithEmail(string email);
    Task<Entities.User?> GetUserByEmail(string email);
    Task<Entities.User?> GetUserById(Guid id);
    Task<bool> ExistsActiveUser(Guid id);
}
