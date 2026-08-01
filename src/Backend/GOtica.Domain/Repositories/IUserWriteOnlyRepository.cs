using GOtica.Domain.Entities;

namespace GOtica.Domain.Repositories;

public interface IUserWriteOnlyRepository
{
    Task Add(User user);
}
