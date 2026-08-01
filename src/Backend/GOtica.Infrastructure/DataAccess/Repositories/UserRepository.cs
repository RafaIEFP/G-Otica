using GOtica.Domain.Entities;
using GOtica.Domain.Repositories;

namespace GOtica.Infrastructure.DataAccess.Repositories;

internal sealed class UserRepository : IUserWriteOnlyRepository
{
    private readonly GOticaDbContext _dbContext;
    public UserRepository(GOticaDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(User user) => await _dbContext.Users.AddAsync(user);
}
