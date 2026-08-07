namespace GOtica.Domain.Repositories.User;

public interface IUserUpdateOnlyRepository
{
    Task<Entities.User> GetUserById(Guid id);
    void Update(Entities.User user);
}
