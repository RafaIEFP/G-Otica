using GOtica.Domain.Entities;
using GOtica.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace GOtica.Infrastructure.DataAccess.Repositories;

internal sealed class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository
{
    private readonly GOticaDbContext _dbContext;
    public UserRepository(GOticaDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(User user) => await _dbContext.Users.AddAsync(user);

    public async Task<bool> ExistActiveUserWithEmail(string email) 
        => await _dbContext.Users.AnyAsync(user => user.Email.Equals(email) && user.IsActive);

    public async Task<User?> GetUserByEmail(string email) 
        => await _dbContext.Users.FirstOrDefaultAsync(user => user.Email.Equals(email) && user.IsActive);

    public async Task<User?> GetUserById(Guid id)
        => await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
}
