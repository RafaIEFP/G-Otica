namespace GOtica.Domain.Repositories.User;

public interface IUserUpdateOnlyRepository
{
    Task<Entities.User> GetUserById(Guid id);
    void Update(Entities.User user);
    Task DeactivateAccount(Guid userId);
    Task ReactivateAccount(Guid userId);
}
