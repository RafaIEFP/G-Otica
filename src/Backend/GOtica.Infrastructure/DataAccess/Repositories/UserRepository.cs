using GOtica.Domain.Entities;
using GOtica.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace GOtica.Infrastructure.DataAccess.Repositories;

internal sealed class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository, IUserUpdateOnlyRepository
{
    private readonly GOticaDbContext _dbContext;
    public UserRepository(GOticaDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(User user) => await _dbContext.Users.AddAsync(user);

    public async Task<bool> ExistActiveUserWithEmail(string email) 
        => await _dbContext.Users.AnyAsync(user => user.Email.Equals(email) && user.IsActive);

    public async Task<User?> GetUserByEmail(string email) 
        => await _dbContext.Users.FirstOrDefaultAsync(user => user.Email.Equals(email) && user.IsActive);

    async Task<User?> IUserReadOnlyRepository.GetUserById(Guid id)
         => await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

    async Task<User> IUserUpdateOnlyRepository.GetUserById(Guid id)
        => await _dbContext.Users.FirstAsync(u => u.Id == id);

    public void Update(User user)
        => _dbContext.Users.Update(user);

    public async Task DeleteAccount(Guid userId)
        => await _dbContext.Users.Where(u => u.Id == userId).ExecuteDeleteAsync();

    public async Task<bool> ExistsActiveUser(Guid id)
        => await _dbContext.Users.AnyAsync(u => u.Id == id && u.IsActive);

    public async Task DeactivateAccount(Guid userId)
    => await _dbContext.Users
        .Where(u => u.Id == userId)
        .ExecuteUpdateAsync(
            setter => setter.SetProperty(u => u.IsActive, false)
        );
}
